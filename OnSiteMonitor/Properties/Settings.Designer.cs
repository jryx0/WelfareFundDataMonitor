﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnSiteFundComparer.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("InputData")]
        public string InputDBFile {
            get {
                return ((string)(this["InputDBFile"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ResultData")]
        public string ResultDBFile {
            get {
                return ((string)(this["ResultDBFile"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string WorkDir {
            get {
                return ((string)(this["WorkDir"]));
            }
            set {
                this["WorkDir"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data")]
        public string WorkingDB {
            get {
                return ((string)(this["WorkingDB"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ResultDir {
            get {
                return ((string)(this["ResultDir"]));
            }
            set {
                this["ResultDir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"CREATE TABLE IF NOT EXISTS refertable
(
RowID      INTEGER      PRIMARY KEY AUTOINCREMENT,
ID         VARCHAR (20),
sRelateID  VARCHAR (20),
sDataDate  DATETIME (0),
InputID    VARCHAR (20),
Name       VARCHAR (20),
Region       VARCHAR (20),
Addr       VARCHAR (20),
DataDate   VARCHAR (20),
Amount     DOUBLE (0),
AmountType VARCHAR (20),
RelateID   VARCHAR (20),
RelateName VARCHAR (20),
Relation   VARCHAR (20),
Type       VARCHAR (20),
ItemType       VARCHAR (20),
Number     VARCHAR (20),
Area       DOUBLE (0),
DataDate1 varchar(20),
Serial1    varchar(30),
Serial2    varchar(30) 
); 

DROP INDEX IF EXISTS index_refertable; 

CREATE INDEX index_refertable ON refertable
(
RowID ASC,
ID ASC
); ")]
        public string ReferTableSql {
            get {
                return ((string)(this["ReferTableSql"]));
            }
            set {
                this["ReferTableSql"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ImportData")]
        public string ImportDBFile {
            get {
                return ((string)(this["ImportDBFile"]));
            }
            set {
                this["ImportDBFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CurrentRegion {
            get {
                return ((string)(this["CurrentRegion"]));
            }
            set {
                this["CurrentRegion"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CurentRegionName {
            get {
                return ((string)(this["CurentRegionName"]));
            }
            set {
                this["CurentRegionName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1.5")]
        public string Version {
            get {
                return ((string)(this["Version"]));
            }
            set {
                this["Version"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public string IsFirst {
            get {
                return ((string)(this["IsFirst"]));
            }
            set {
                this["IsFirst"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data\\config.Uncryt")]
        public string MainDBFile {
            get {
                return ((string)(this["MainDBFile"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://156.18.1.93/")]
        public string WebServicesUrl {
            get {
                return ((string)(this["WebServicesUrl"]));
            }
            set {
                this["WebServicesUrl"] = value;
            }
        }
    }
}
