import datetime
import argparse
from urllib.parse import quote_plus
from migration_utils import DVHelper, GraphHelper, CertHelper, Console


# FIC Name
fic_name = "PPMI_FIC_V1"

entra_issuers = {
    "public": "login.microsoftonline.com",
    "gcc": "login.microsoftonline.us",
    "gcch": "login.microsoftonline.us",
    "dod": "login.microsoftonline.us",
    "usnat": "login.microsoftonline.us",
    "ussec": "login.microsoftonline.us",
    "cn": "login.partner.microsoftonline.cn",
}

def perform_fic_operations_graph(app_client_id, app_tenant_id, generate_fic):
    # Use msal + requests through GraphHelper to check and (optionally) add the FIC.
    
    print("Connecting to Microsoft Graph (msal + requests) via GraphHelper...")
    graph = GraphHelper(app_tenant_id)

    try:
        # Find the application object by appId (client/app client id)
        resp = graph.get(f"/applications?$filter=appId eq '{app_client_id}'")
        apps = resp.json().get("value", [])
        update_app_object_id = apps[0].get("id") if apps else None
        
        Console.exit_if(not update_app_object_id, "Application not found.")

        # List existing federated identity credentials
        resp = graph.get(f"/applications/{update_app_object_id}/federatedIdentityCredentials")
        existing_fics = resp.json().get("value", [])
        fic_exists = next((f for f in existing_fics if f.get("name") == fic_name), None)

        if not fic_exists:
            current_date = datetime.datetime.utcnow().strftime("%Y-%m-%d")
            description = f"PPMI FIC V1 Added using migration script on {current_date}"
            audience = "api://AzureADTokenExchange"
            fic_iss, fic_sub = generate_fic()
            payload = {
                "name": fic_name,
                "issuer": fic_iss,
                "subject": fic_sub,
                "audiences": [audience],
                "description": description
            }

            post_resp = graph.post(f"/applications/{update_app_object_id}/federatedIdentityCredentials", json=payload)
            Console.exit_if(post_resp.status_code not in (200, 201), f"Failed to add FIC: {post_resp.status_code} - {post_resp.text}")
            print(f"Federated Identity Credential '{fic_name}' added.")
        else:
            print("V1 FIC already exists.")
    except Exception as ex:
        Console.exit_if(True, f"Failed to validate and add FIC: {ex}")

def perform_fic_operations_manual(app_client_id, generate_fic):
    print(f"Please add or verify the new FIC in the Application {app_client_id} in Entra.")
    print(f"1. Browse to 'https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/~/Credentials/appId/{app_client_id}/isMSAApp~/false'.")
    print(f"2. Select Federated credentials tab.")
    print(f"3. Confirm that the FIC with Name 'PPMI_FIC_V1' exists with the sub identifier starting with '/eid1/...'")
    fic_added = Console.yes_no("Is FIC already added?")
    if fic_added:
        return True
    else:
        # Generate new FIC
        fic_iss, fic_sub = generate_fic()
        
        print(f"Please add a new FIC to your application with the following details:")
        print()
        Console.highlight(f"Name: {fic_name}")
        Console.highlight(f"Issuer: {fic_iss}")
        Console.highlight(f"Subject: {fic_sub}")
        print()
        print(f"Continue after FIC has been added or exit and restart the script after the FIC has been added.")
        fic_added = Console.yes_no("Do you want to continue?")
        if fic_added:
            return True
        else:
            return False

def main():
    # Process Arguments
    parser = argparse.ArgumentParser(description="Migrate Plugin MI to V1")
    parser.add_argument("--org", "-o", help="Dataverse org URL or org name (e.g., org.crm.dynamics.com or OrgName)", required=True)
    parser.add_argument("--managed-identity-id", "-m", help="Managed Identity ID (GUID) to migrate", required=True)
    parser.add_argument("--environment-type", "-e", help="Environment type (e.g., Test, PreProd, Prod)", required=False, default="Prod")
    parser.add_argument("--sov-cloud", "-s", help="Azure Sovereign Cloud (gcc, gcch, dod, usnat, ussec, cn, ...)", required=False, choices=["gcc", "gcch", "dod", "usnat", "ussec", "cn"])
    args = parser.parse_args()

    # Initialize all variables based on org url and managed identity id
    dv = DVHelper(args.org)
    managed_identity_id = args.managed_identity_id
    environment_type = args.environment_type
    cloud = args.sov_cloud if args.sov_cloud else "public"

    # Dataverse login
    dv.login()

    response = dv.get(f"WhoAmI")
    environment_id = response.get("OrganizationId")

    # Get managed-identity record
    managed_identity = dv.get(f"managedidentities({managed_identity_id})")
    app_client_id = managed_identity.get("applicationid")
    app_tenant_id = managed_identity.get("tenantid")

    # Check if the managed identity exists and if it requires an update
    Console.exit_if(not managed_identity, f"Managed Identity with ID {managed_identity_id} not found.")

    # Information collected. Proceeding with update.
    # ----------------------------------------------
    print(f"Org Url: {dv.org}")
    print(f"Tenant ID: {dv.tenant_id}")
    print(f"Environment Type: {environment_type}")
    print(f"Managed Identity ID: {managed_identity_id}")
    print(f"Application (Client) ID: {app_client_id}")
    print(f"App Tenant ID: {app_tenant_id}")

    if managed_identity.get("version") != 0:
        print("Managed Identity is already at version 1.")
        revert = Console.yes_no("Do you want to revert the version to 0?")
        if revert:
            dv.patch(f"managedidentities({managed_identity_id})", {"version": 0})
            print("Managed Identity version reverted to 0 successfully.")
            print("Please re-run the script to continue with migration process.")
        else:
            print("No further action needed for migration of this managed identity.")
    else:
        # Define FIC generation
        def generate_fic():
            # Generate new FIC
            print("Generating V1 FIC...")
            # Code to generate new FIC goes here
            print("Access to the certificate used to sign the Plugin assembly/package is needed to generate the V1 FIC.")
            print("Please select the certificate file (PFX or CER) used to sign the Plugin assembly/package.")
            cert_file = Console.select_file(filetypes=[("Certificate Files", "*.pfx;*.cer")])
            
            Console.exit_if(not cert_file, "No certificate file selected.")

            #read cert details
            subject, issuer, is_self_signed, sha256_hex = CertHelper.get_cert_info(cert_file)
            encoded_tenant_id = quote_plus(dv.tenant_id)
            ppmi_app_ids = {
                "Test": "L5f3f5fVhEuUXYRgAT1Q4w", # Test
                "PreProd": "CQSGf3JJtEi27nY2ePL7UQ", # PreProd
                "Prod": "qzXoWDkuqUa3l6zM5mM0Rw", # Prod
            }

            fic_audiences = {
                "public": "api://AzureADTokenExchange",
                "gcc": "api://AzureADTokenExchange",
                "gcch": "api://AzureADTokenExchangeUSGov",
                "dod": "api://AzureADTokenExchangeUSGov",
                "usnat": "api://AzureADTokenExchangeUSNat",
                "ussec": "api://AzureADTokenExchangeUSSec",
                "cn": "api://AzureADTokenExchangeChina",
            }

            encoded_app_id = quote_plus(ppmi_app_ids[environment_type], "")
            fic_aud = fic_audiences[cloud]

            fic_iss = f"https://{entra_issuers[cloud]}/{dv.tenant_id}/v2.0"
            cert_fmi = f"h/{sha256_hex}" if is_self_signed else f"i/{issuer}/s/{subject}"
            fic_sub = f"/eid1/c/pub/t/{encoded_tenant_id}/a/{encoded_app_id}/n/plugin/e/{environment_id}/{cert_fmi}"
            return fic_iss, fic_sub, fic_aud

        # Step 1: Check/Add FIC - decide if automatic or manual
        print(f"V1 FIC must be added to the Application {app_client_id} (in Tenant {app_tenant_id}) before updating the managed identity version to 1.")
        print(f"You can choose to access Entra App registration to verify and add the new FIC. Otherwise, you can add it manually using a browser.")
        input = Console.yes_no("Continue with automatic update?")

        if input:
            perform_fic_operations_graph(app_client_id, app_tenant_id, generate_fic)
        else:
            perform_fic_operations_manual(app_client_id, generate_fic)

        # FIC added, update managed identity version
        print(("We can now update the managed identity version to to 1. Please confirm to perform the update."))
        confirm = Console.yes_no("Continue?")

        if confirm:
            dv.patch(f"managedidentities({managed_identity_id})", {"version": 1})
        else:
            print("Update canceled. Please run the script again to continue.")


if __name__ == "__main__":
    main()