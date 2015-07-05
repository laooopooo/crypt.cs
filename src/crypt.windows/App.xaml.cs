/// @file
/// Implementation of the `Crypt.Windows.App` class.

namespace Crypt.Windows {
  using System;
  using System.Globalization;
  using System.Linq;
  using System.Threading;
  using System.Windows;

  using Crypt.Windows.Properties;
  using Messages=Crypt.Windows.Properties.Resources;

  using MiniFramework.Reflection;
  using MiniFramework.Text;
  using MiniFramework.Windows;

  /// The WPF application.
  internal partial class App: Application {
  
    /// Initializes a new instance of the class.
    public App() {
      this.Properties["Authors"]="Cédric Belin <cedric@belin.io>";
      this.Properties["License"]=OpenSourceLicenses.GnuGeneralPublicLicenseV3;
      this.Properties["Product"]=new AssemblyInfo(this.GetType().Assembly).Product;
    }

    /// Checks that all conditions are met for application startup.
    /// If no string encoder can be found, a message is displayed to the user and the application terminated.
    private void CheckSetup() {
      if(EncoderManager.Encoders.Count==0) {
        var message=Messages.AddInsNotFoundError.Split('|');
        TaskDialog.Show(null, message[1], message[0], null, MessageBoxButton.OK, MessageBoxImage.Error);
        this.Shutdown(1);
      }
    }

    /// Saves the application settings on exit.
    /// @param sender The source of the event.
    /// @param e The event data.
    private void OnApplicationExit(object sender, ExitEventArgs e) {
      Settings.Default.Save();
    }

    /// Sets the application culture on startup.
    /// @param sender The source of the event.
    /// @param e The event data.
    private void OnApplicationStartup(object sender, StartupEventArgs e) {
      var culture=Settings.Default.Culture;
      var languages=Settings.Default.SupportedLanguages;

      if(culture.Equals(CultureInfo.InvariantCulture) || !languages.Contains(culture.TwoLetterISOLanguageName)) {
        var isoCode=(languages.Contains(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)) ?
          CultureInfo.CurrentUICulture.TwoLetterISOLanguageName : Settings.Default.SupportedLanguages[0];

        culture=CultureInfo.CreateSpecificCulture(isoCode);
      }

      Thread.CurrentThread.CurrentCulture=culture;
      Thread.CurrentThread.CurrentUICulture=culture;
      this.CheckSetup();
    }
  }
}