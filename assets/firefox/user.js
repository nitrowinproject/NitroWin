/*
* Kind of hardened user.js from NitroWin
* https://github.com/Nitro4542/NitroWin/
*/

// New tab/homepage settings
user_pref("browser.startup.page", 1);
user_pref("browser.startup.homepage", "about:home");

user_pref("browser.newtabpage.enabled", true);

user_pref("browser.newtabpage.activity-stream.showSponsored", false);
user_pref("browser.newtabpage.activity-stream.showSponsoredTopSites", false);

// Clear sponsored shortcuts
user_pref("browser.newtabpage.activity-stream.default.sites", "");

// Disable geolocation
user_pref("geo.provider.ms-windows-location", false);

// Disable personalized recommendations
user_pref("extensions.getAddons.showPane", false);

user_pref("extensions.htmlaboutaddons.recommendations.enabled", false);

user_pref("browser.discovery.enabled", false);

// Disable telemetry
// Disable new data submission
user_pref("datareporting.policy.dataSubmissionEnabled", false);

// Disable Health Reports
user_pref("datareporting.healthreport.uploadEnabled", false);

user_pref("toolkit.telemetry.enabled", false);
user_pref("toolkit.telemetry.unified", false);
user_pref("toolkit.telemetry.server", "data:,");
user_pref("toolkit.telemetry.archive.enabled", false);
user_pref("toolkit.telemetry.newProfilePing.enabled", false);
user_pref("toolkit.telemetry.shutdownPingSender.enabled", false);
user_pref("toolkit.telemetry.updatePing.enabled", false);
user_pref("toolkit.telemetry.bhrPing.enabled", false);
user_pref("toolkit.telemetry.firstShutdownPing.enabled", false);
user_pref("toolkit.telemetry.coverage.opt-out", true);
user_pref("toolkit.coverage.opt-out", true);
user_pref("toolkit.coverage.endpoint.base.", "");

user_pref("browser.newtabpage.activity-stream.feeds.telemetry", false);
user_pref("browser.newtabpage.activity-stream.telemetry", false);

// Disable studies
user_pref("app.shield.optoutstudies.enabled", false);

user_pref("app.normandy.enabled", false);
user_pref("app.normandy.api_url", "");

// Disable crash reports
user_pref("breakpad.reportURL", "");
user_pref("browser.tabs.crashReporting.sendReport", false);

// Disable link prefetching
user_pref("network.prefetch-next", false);

// Disable predictor
user_pref("network.predictor.enabled", false);

// Remove special permissions for certain mozilla domains
user_pref("permissions.manager.defaultsUrl", "");

// Use Punycode in Internationalized Domain Names to eliminate possible spoofing
user_pref("network.IDN_show_punycode", true);

// Disable using UNC (Uniform Naming Convention) paths (prevent proxy bypass)
user_pref("network.file.disable_unc_paths", true);

// Disable location bar making speculative connections
user_pref("browser.urlbar.speculativeConnect.enabled", false);

// Disable more ads
user_pref("browser.urlbar.suggest.quicksuggest.sponsored", false);

// Disable urlbar trending search suggestions
user_pref("browser.urlbar.trending.featureGate", false);

// Disable Form Autofill
user_pref("extensions.formautofill.addresses.enabled", false);
user_pref("extensions.formautofill.creditCards.enabled", false);

// Disable saving passwords
user_pref("signon.rememberSignons", false);

// Disable autofill login and passwords
user_pref("signon.autofillForms", false);

// Disable formless login capture for Password Manager
user_pref("signon.formlessCapture.enabled", false);

// Disable media cache from writing to disk in Private Browsing
user_pref("browser.privatebrowsing.forceMediaMemoryCache", true);

// Disable resuming session from crash
user_pref("browser.sessionstore.resume_from_crash", false);

// Disable automatic Firefox start and session restore after reboot [Windows]
user_pref("toolkit.winRegisterApplicationRestart", false);

// Disable page thumbnail collection
user_pref("browser.pagethumbnails.capturing_disabled", true);

// Delete temporary files opened from non-Private Browsing windows with external apps
user_pref("browser.download.start_downloads_in_tmp_dir", true);
user_pref("browser.helperApps.deleteTempFileOnExit", true);

// Display advanced information on Insecure Connection warning pages
user_pref("browser.xul.error_pages.expert_bad_cert", true);

// Prevent mitm
user_pref("security.cert_pinning.enforcement_level", 2);

// Enable CRLite
user_pref("security.remote_settings.crlite_filters.enabled", true);
user_pref("security.pki.crlite_mode", 2);

// Disable adding downloads to system's "recent documents" list
user_pref("browser.download.manager.addToRecentDocs", false);

/* Enable Containers and show the UI settings
 * https://wiki.mozilla.org/Security/Contextual_Identity_Project/Containers */
user_pref("privacy.userContext.enabled", true);
user_pref("privacy.userContext.ui.enabled", true);

