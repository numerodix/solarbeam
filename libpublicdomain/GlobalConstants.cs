using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Various useful global constants.
    /// </summary>
    public static class GlobalConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string PublicDomainMainVersion = "0.2.50";

        /// <summary>
        /// 
        /// </summary>
        public const string PublicDomainBuildVersion = "0";

        /// <summary>
        /// Current version of this code, in string form. In a standalone build,
        /// this is the assembly version.
        /// </summary>
        public const string PublicDomainVersion = PublicDomainMainVersion + ".0";

        /// <summary>
        /// Current version of this code, in string form. In a standalone build,
        /// this is the file version.
        /// </summary>
        public const string PublicDomainFileVersion = PublicDomainMainVersion + "." + PublicDomainBuildVersion;

        /// <summary>
        /// The name of the PublicDomain assembly, if this is a standalone build. If
        /// this file is included in an existing project, this is purely a logical name.
        /// </summary>
        public const string PublicDomainName = "PublicDomain";

        /// <summary>
        /// Strong, public name of the PublicDomain assembly, if this is a standalone
        /// build. If this file is included in an existing project, this is meaningless.
        /// </summary>
        public const string PublicDomainStrongName = PublicDomainName + ", Version=" + PublicDomainVersion + ", Culture=neutral, PublicKeyToken=FD3F43B5776A962B";

        /// <summary>
        /// Fully qualified, absolute URL which acts as a namespace for the classes in the
        /// PublicDomain.
        /// Always ends in a trailing slash.
        /// </summary>
        public const string PublicDomainNamespace = "http://www.codeplex.com/PublicDomain/";
    }
}