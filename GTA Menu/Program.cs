using System;
using System.Windows.Forms;
using System.Diagnostics;

using static GTA_Menu.HotKeyManager;

namespace GTA_Menu {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GTAOnlineSuspender());
        }
    }

    public class GTAOnlineSuspender : ApplicationContext {
        private NotifyIcon trayIcon;

        public GTAOnlineSuspender() {
            trayIcon = new NotifyIcon();

            RegisterKey(Keys.S, KeyModifiers.Alt | KeyModifiers.Control);
            RegisterKey(Keys.X, KeyModifiers.Alt | KeyModifiers.Control);
            HotKeyPressed += new EventHandler<HotKeyEventArgs>(OnHotKeyPressed);

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem("Suspend GTA", (object sender, EventArgs e) => SuspendTask(), (Shortcut)(Keys.Control | Keys.Alt | Keys.S)));
            contextMenu.MenuItems.Add(new MenuItem("Force Quit GTA", (object sender, EventArgs e) => ForceEndTask(), (Shortcut)(Keys.Control | Keys.Alt | Keys.X)));
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(new MenuItem("Exit", (object sender, EventArgs e) => { trayIcon.Visible = false; Application.Exit(); }));

            trayIcon.Icon = Properties.Resources.SuspendIcon;
            trayIcon.ContextMenu = contextMenu;
            trayIcon.Visible = true;
        }

        void OnHotKeyPressed(object sender, HotKeyEventArgs args) {
            switch (args.Key) {
                case Keys.X: ForceEndTask(); break;
                case Keys.S: SuspendTask(); break;
                default: break;
            }
        }

        public void ForceEndTask() {
            Process[] processes = Process.GetProcessesByName("GTA5");
            if (processes.Length == 0) return;
            Hacks.ForceEndTask(processes[0]);
        }

        public void SuspendTask() {
            Process[] processes = Process.GetProcessesByName("GTA5");
            if (processes.Length == 0) return;
            Hacks.SuspendTask(processes[0]);
        }
    }
}
