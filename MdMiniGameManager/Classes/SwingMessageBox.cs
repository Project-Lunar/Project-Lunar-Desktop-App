using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectLunarUI
{
    public static class SwingMessageBox
    {
        // Possible icon choices
        // "chat":"icn_chat"
        // "construction":"icn_construction"
        // "download":"icn_download"
        // "electric":"icn_elec"
        // "electron":"icn_electron"
        // "fire":"icn_fire"
        // "info":"icn_info"
        // "announcement":"icn_megaphone"
        // "noaccess":"icn_noaccess"
        // "nuke":"icn_nuke"
        // "options":"icn_option"
        // "sdcard":"icn_sdcard"
        // "stop":"icn_stop"
        // "upload":"icn_upload"
        // "warn":"icn_warn"
        // "wizard":"icn_wizard"

        // Possible dialog types
        // OK (OK)
        // OKCAN (OK, Cancel)
        // YESNO (Yes, No)
        // RETRY (Retry, Cancel)

        //public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton);
        //public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons);
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            string swingButtons = GetSwingButtons(buttons);
            string swingIcon = GetSwingIcon(icon);

            frmMsgDialog msgDialog = new frmMsgDialog(swingIcon, caption, text, swingButtons);
            return msgDialog.ShowDialog(owner);
        }

        private static string GetSwingIcon(MessageBoxIcon icon)
        {
            string swingIcon = string.Empty;
            if (icon.Equals(MessageBoxIcon.None))
            {
                swingIcon = "checkmark";
            }
            else if (icon.Equals(MessageBoxIcon.Hand))
            {
                swingIcon = "stop";
            }
            else if (icon.Equals(MessageBoxIcon.Question))
            {
                swingIcon = "question";
            }
            else if (icon.Equals(MessageBoxIcon.Exclamation))
            {
                swingIcon = "warn";
            }
            else if (icon.Equals(MessageBoxIcon.Asterisk))
            {
                swingIcon = "wizard";
            }
            else if (icon.Equals(MessageBoxIcon.Stop))
            {
                swingIcon = "noaccess";
            }
            else if (icon.Equals(MessageBoxIcon.Error))
            {
                swingIcon = "stop";
            }
            else if (icon.Equals(MessageBoxIcon.Warning))
            {
                swingIcon = "nuke";
            }
            else if (icon.Equals(MessageBoxIcon.Information))
            {
                swingIcon = "info";
            }
            else
            {
                swingIcon = "info";
            }

            return swingIcon;
        }

        private static string GetSwingButtons(MessageBoxButtons buttons)
        {
            string swingButtons = string.Empty;
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    swingButtons = "OK";
                    break;
                case MessageBoxButtons.OKCancel:
                    swingButtons = "OKCAN";
                    break;
                case MessageBoxButtons.YesNo:
                    swingButtons = "YESNO";
                    break;
                case MessageBoxButtons.RetryCancel:
                    swingButtons = "RETRY";
                    break;
                case MessageBoxButtons.YesNoCancel:
                    swingButtons = "YESNOCAN";
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                default:
                    swingButtons = "OK";
                    break;
            }

            return swingButtons;
        }

        //public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
        //public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options);
        public static DialogResult Show(string text)
        {
            return Show(null, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        //public static DialogResult Show(string text, string caption);
        //public static DialogResult Show(string text, string caption, MessageBoxButtons buttons);
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, text, caption, buttons, icon);        
        }
        //public static DialogResult Show(IWin32Window owner, string text, string caption);
        //public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
        //public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param);
        //public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param);
        //public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator);
        //public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator);
        //public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword);
        //public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword);
        //public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath);
        //public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath);
        //public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options);
        //public static DialogResult Show(IWin32Window owner, string text);
    }
}
