﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DiscordQueryBot {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.6.0.0")]
    internal sealed partial class DiscordQuery : global::System.Configuration.ApplicationSettingsBase {
        
        private static DiscordQuery defaultInstance = ((DiscordQuery)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new DiscordQuery())));
        
        public static DiscordQuery Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string BotToken {
            get {
                return ((string)(this["BotToken"]));
            }
            set {
                this["BotToken"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection EmbedDescriptions {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["EmbedDescriptions"]));
            }
            set {
                this["EmbedDescriptions"] = value;
            }
        }
    }
}
