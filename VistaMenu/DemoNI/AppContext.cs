using System;
using System.Windows.Forms;
using wyDay.Controls;

namespace VistaMenuDemoNI
{
    internal class AppContext : ApplicationContext
    {
        private NotifyIcon notifyIcon;

        public AppContext() {
            VistaMenu vistaMenu = new VistaMenu();


            // Creates menu items for the notify icon context menu
            MenuItem mnuShow = new MenuItem("Show", new EventHandler(mnuShow_Click));
            MenuItem mnuSettings = new MenuItem("Settings");
            MenuItem mnuSeparator1 = new MenuItem("-");
            MenuItem mnuAbout = new MenuItem("About...");
            MenuItem mnuSeparator2 = new MenuItem("-");
            MenuItem mnuExit = new MenuItem("Exit", new EventHandler(mnuExit_Click));

            // Adds the menu items to the notify icon context menu
            ContextMenu ctmNotifyIcon = new ContextMenu(new MenuItem[] {
                mnuShow, mnuSettings, mnuSeparator1, mnuAbout, mnuSeparator2, mnuExit
            });


            //Setup the VistaMenu
            vistaMenu.SetImage(mnuSettings, Properties.Resources.Settings);
            vistaMenu.SetImage(mnuAbout, Properties.Resources.About);


            //EndInit() is called by the designer on forms, but since this
            //is an ApplicationContext you need to call it manually
            ((System.ComponentModel.ISupportInitialize)(vistaMenu)).EndInit();



            // Creates a new instance for the notify icon
            notifyIcon = new NotifyIcon() {
                Icon = Properties.Resources.Star,
                ContextMenu = ctmNotifyIcon,
                Visible = true,
            };
        }

        private void mnuShow_Click(object sender, EventArgs e) {
            MessageBox.Show("Do something here.", "Do something...", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuExit_Click(object sender, EventArgs e) {
            notifyIcon.Dispose();
            Application.Exit();
        }
    }
}
