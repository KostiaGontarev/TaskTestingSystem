﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TTS.UI.UserControls
{
	/// <summary>
	/// Interaction logic for Exec_file.xaml
	/// </summary>
	public partial class ExecutedFile : UserControl
    {
        #region Members
        private string filePath;
        #endregion

        #region Properties
        public string FilePath
        {
            get
            {
                return filePath;
            }
        }
        #endregion

        #region Constructors
        public ExecutedFile()
		{
			this.InitializeComponent();
        }

        public ExecutedFile(string filePath)
            :this()
        {
            this.filePath = filePath;
            FilePathLabel.Content = this.filePath;
        }
        #endregion

    }
}