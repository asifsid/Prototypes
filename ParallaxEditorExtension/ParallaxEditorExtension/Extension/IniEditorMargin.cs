﻿//------------------------------------------------------------------------------
// <copyright file="IniEditorMargin.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.IO;
using System.Globalization;

namespace ParallaxEditorExtension
{
    /// <summary>
    /// Margin's canvas and visual definition including both size and content
    /// </summary>
    internal class IniEditorMargin : Canvas, IWpfTextViewMargin
    {
        /// <summary>
        /// Margin name.
        /// </summary>
        public const string MarginName = "IniEditorMargin";
        
        private bool _isDisposed;
        private IWpfTextView _textView;
        private FrameworkElement _viewContainer;
        private ParallaxOutline _outlineControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="IniEditorMargin"/> class for a given <paramref name="textView"/>.
        /// </summary>
        /// <param name="textView">The <see cref="IWpfTextView"/> to attach the margin to.</param>
        public IniEditorMargin(IWpfTextView textView)
        {
            var docPath = textView.TextDataModel.DocumentBuffer.Properties.GetProperty<Microsoft.VisualStudio.Text.ITextDocument>(typeof(Microsoft.VisualStudio.Text.ITextDocument))?.FilePath;
            var vm = new OutlineViewModel(docPath, textView.TextSnapshot.GetText());
            
            _textView = textView;
            _outlineControl = new ParallaxOutline(vm);
            _viewContainer = (_textView as FrameworkElement).Parent as FrameworkElement;

            _viewContainer.LayoutUpdated += OnParentLayoutChanged;
            textView.TextBuffer.Changed += OnTextBufferChanged;

            ClipToBounds = true;
            Children.Add(_outlineControl);

            OnParentLayoutChanged(null, EventArgs.Empty);
        }

        private void OnParentLayoutChanged(object sender, EventArgs e)
        {
            Width = _viewContainer.ActualWidth / 2;
        }

        #region IWpfTextViewMargin

        /// <summary>
        /// Gets the <see cref="Sytem.Windows.FrameworkElement"/> that implements the visual representation of the margin.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The margin is disposed.</exception>
        public FrameworkElement VisualElement
        {
            // Since this margin implements Canvas, this is the object which renders
            // the margin.
            get
            {
                this.ThrowIfDisposed();
                return this;
            }
        }

        #endregion

        #region ITextViewMargin

        /// <summary>
        /// Gets the size of the margin.
        /// </summary>
        /// <remarks>
        /// For a horizontal margin this is the height of the margin,
        /// since the width will be determined by the <see cref="ITextView"/>.
        /// For a vertical margin this is the width of the margin,
        /// since the height will be determined by the <see cref="ITextView"/>.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The margin is disposed.</exception>
        public double MarginSize
        {
            get
            {
                this.ThrowIfDisposed();

                // Since this is a horizontal margin, its width will be bound to the width of the text view.
                // Therefore, its size is its height.
                return this.ActualHeight;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the margin is enabled.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The margin is disposed.</exception>
        public bool Enabled
        {
            get
            {
                this.ThrowIfDisposed();

                // The margin should always be enabled
                return true;
            }
        }

        /// <summary>
        /// Gets the <see cref="ITextViewMargin"/> with the given <paramref name="marginName"/> or null if no match is found
        /// </summary>
        /// <param name="marginName">The name of the <see cref="ITextViewMargin"/></param>
        /// <returns>The <see cref="ITextViewMargin"/> named <paramref name="marginName"/>, or null if no match is found.</returns>
        /// <remarks>
        /// A margin returns itself if it is passed its own name. If the name does not match and it is a container margin, it
        /// forwards the call to its children. Margin name comparisons are case-insensitive.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="marginName"/> is null.</exception>
        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return string.Equals(marginName, IniEditorMargin.MarginName, StringComparison.OrdinalIgnoreCase) ? this : null;
        }

        /// <summary>
        /// Disposes an instance of <see cref="IniEditorMargin"/> class.
        /// </summary>
        public void Dispose()
        {
            if (!this._isDisposed)
            {
                GC.SuppressFinalize(this);
                this._isDisposed = true;
            }
        }

        #endregion

        private void OnTextBufferChanged(object sender, Microsoft.VisualStudio.Text.TextContentChangedEventArgs e)
        {
            _outlineControl.Content = e.After.GetText();
        }

        /// <summary>
        /// Checks and throws <see cref="ObjectDisposedException"/> if the object is disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException(MarginName);
            }
        }
    }
}
