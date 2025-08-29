"""
Utilities for MigrateMI.py. Common helper functions have been moved here so they can be shared and tested separately.
"""

import sys
import os
import requests
import msal
import jwt
import hashlib
import tkinter as tk
from tkinter import filedialog
from cryptography import x509
from cryptography.hazmat.primitives import serialization
from cryptography.hazmat.primitives.serialization import pkcs12

CLIENT_ID = "04b07795-8ddb-461a-bbee-02f9e1bf7b46"

# Helper class for Dataverse operations
class DVHelper:
    ORG_SUFFIX = ".crm.dynamics.com"

    def __init__(self, org):
        self.org = DVHelper._org_full_name(org)
        self.tenant_id = None
        self.scopes = [f"https://{self.org}/.default"]
        self.app = msal.PublicClientApplication(CLIENT_ID, authority="https://login.microsoftonline.com/common")

    def login(self):
        result = self.app.acquire_token_interactive(scopes=self.scopes)
        if "access_token" in result:
            self.tenant_id = DVHelper._get_tenant_id(result["access_token"])
            self.access_token = result["access_token"]
        else:
            print("Failed to acquire token.")
            sys.exit(1)

    def get(self, uri):
        headers = self._get_headers()
        response = requests.get(self._full_url(uri), headers=headers)
        response.raise_for_status()
        return response.json()

    def patch(self, uri, data):
        headers = self._get_headers(is_patch=True)
        response = requests.patch(self._full_url(uri), headers=headers, json=data)
        response.raise_for_status()
        return response
    
    def _full_url(self, path: str) -> str:
        # Always accept either a relative path
        return "https://" + self.org + "/api/data/v9.2" + (path if path.startswith("/") else "/" + path)

    @staticmethod
    def _org_full_name(org):
        return org if org.endswith(DVHelper.ORG_SUFFIX) else f"{org}{DVHelper.ORG_SUFFIX}"

    @staticmethod
    def _get_tenant_id(access_token):
        """Decode a JWT access token (without verification) and return the tenant id (tid)."""
        try:
            payload = jwt.decode(access_token, options={"verify_signature": False})
            return payload.get("tid")
        except Exception as ex:
            print(f"Failed to decode token: {ex}")
            sys.exit(1)

    def _get_headers(self, is_patch=False):
        headers = {
            "Authorization": f"Bearer {self.access_token}",
            "Content-Type": "application/json",
            "OData-MaxVersion": "4.0",
            "OData-Version": "4.0"
        }
        if is_patch:
            headers["If-Match"] = "*"
        return headers

# Certificate helper class
class CertHelper:
    @staticmethod
    def get_cert_info(path, pfx_password=None):
        ext = os.path.splitext(path)[1].lower()
        cert = None

        if ext in (".pfx", ".p12"):
            with open(path, "rb") as f:
                pfx_bytes = f.read()
            pwd = pfx_password.encode() if pfx_password else None
            _, cert, _ = pkcs12.load_key_and_certificates(pfx_bytes, pwd)
            if cert is None:
                raise ValueError("No certificate found in PFX")

        elif ext in (".cer", ".crt", ".pem"):
            with open(path, "rb") as f:
                data = f.read()
            if b"-----BEGIN CERTIFICATE-----" in data:
                cert = x509.load_pem_x509_certificate(data)
            else:
                cert = x509.load_der_x509_certificate(data)

        else:
            raise ValueError("Unsupported certificate file type. Use .pfx, .cer, .crt or .pem")

        subject = cert.subject.rfc4514_string()
        issuer = cert.issuer.rfc4514_string()
        is_self_signed = (subject == issuer)

        der_bytes = cert.public_bytes(serialization.Encoding.DER)
        sha256_hex = hashlib.sha256(der_bytes).hexdigest()
        
        return subject, issuer, is_self_signed, sha256_hex

# GraphHelper class to simplify Graph API calls
class GraphHelper:
    """Acquire a Graph access token via MSAL and provide convenience get/post methods.

    Example usage:
      g = GraphHelper(tenant_id)
      resp = g.get(f"/applications?$filter=appId eq '{app_client_id}'")
    """
    def __init__(self, tenant_id):
        self.tenant_id = tenant_id
        self.scopes = ["Application.ReadWrite.All"]
        self.app = msal.PublicClientApplication(CLIENT_ID, authority=f"https://login.microsoftonline.com/{tenant_id}")
        self._acquire_token()

    def _acquire_token(self):
        result = self.app.acquire_token_interactive(scopes=self.scopes)
        if not result or "access_token" not in result:
            raise SystemExit("Failed to acquire Graph token.")
        self.access_token = result["access_token"]
        self.headers = {"Authorization": f"Bearer {self.access_token}", "Content-Type": "application/json"}

    def _full_url(self, path: str) -> str:
        # Accept either a relative Graph path (starting with '/') or a full URL
        if path.lower().startswith("http"):
            return path
        base = "https://graph.microsoft.com/v1.0"
        return path if path.startswith(base) else base + (path if path.startswith("/") else "/" + path)

    def get(self, path: str, params: dict = None):
        url = self._full_url(path)
        resp = requests.get(url, headers=self.headers, params=params)
        resp.raise_for_status()
        return resp

    def post(self, path: str, json: dict = None):
        url = self._full_url(path)
        resp = requests.post(url, headers=self.headers, json=json)
        # let caller decide how to handle status codes
        return resp

class Console:
    @staticmethod
    def highlight(message: str):
        print(f"\033[1;33m{message}\033[0m")

    @staticmethod
    def exit_if(condition: bool, message: str):
        if condition:
            print(f"\033[1;91m{message}\033[0m")
            sys.exit(1)

    @staticmethod
    def yes_no(message: str):
        response = input(f"{message} [y/n]: ").strip().lower()
        while True:
            if response == "y":
                return True
            elif response == "n":
                return False
            response = input("Invalid input. Please enter 'y' or 'n' (or x to exit): ").strip().lower()
            if response == "x":
                sys.exit(0)

    @staticmethod
    def select_file(filetypes: str):
         # use a file picker dialog to select the file
        root = tk.Tk()
        root.withdraw()  # Hide the root window
        file = filedialog.askopenfilename(filetypes=filetypes)
        root.destroy()

        return file