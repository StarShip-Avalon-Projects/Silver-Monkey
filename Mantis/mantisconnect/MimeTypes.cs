//-----------------------------------------------------------------------
// <copyright file="MimeTypes.cs" company="Victor Boctor">
//     Copyright (C) All Rights Reserved
// </copyright>
// <summary>
// MantisConnect is copyrighted to Victor Boctor
//
// This program is distributed under the terms and conditions of the GPL
// See LICENSE file for details.
//
// For commercial applications to link with or modify MantisConnect, they require the
// purchase of a MantisConnect commercial license.
// </summary>
//-----------------------------------------------------------------------

namespace SilverMonkey.BugTraqConnect
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A utility class that maps file extensions to mime types.
    /// </summary>
    public static class MimeTypes
    {
        #region Private Fields

        /// <summary>
        /// The default mime type to use when the extension is not mapped.
        /// </summary>
        private const string DefaultMimeType = "application/octet-stream";

        /// <summary>
        /// The dictionary for mime types.
        /// </summary>
        private static IDictionary<string, string> mimeTypes;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Static constructor for <see cref="MimeTypes"/>.
        /// </summary>
        static MimeTypes()
        {
            mimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            mimeTypes[".3dm"] = "x-world/x-3dmf";
            mimeTypes[".3dmf"] = "x-world/x-3dmf";
            mimeTypes[".a"] = "application/octet-stream";
            mimeTypes[".aab"] = "application/x-authorware-bin";
            mimeTypes[".aam"] = "application/x-authorware-map";
            mimeTypes[".aas"] = "application/x-authorware-seg";
            mimeTypes[".abc"] = "text/vnd.abc";
            mimeTypes[".acgi"] = "text/html";
            mimeTypes[".afl"] = "video/animaflex";
            mimeTypes[".ai"] = "application/postscript";
            mimeTypes[".aif"] = "audio/aiff";
            mimeTypes[".aifc"] = "audio/aiff";
            mimeTypes[".aiff"] = "audio/aiff";
            mimeTypes[".aim"] = "application/x-aim";
            mimeTypes[".aip"] = "text/x-audiosoft-intra";
            mimeTypes[".ani"] = "application/x-navi-animation";
            mimeTypes[".aos"] = "application/x-nokia-9000-communicator-add-on-software";
            mimeTypes[".aps"] = "application/mime";
            mimeTypes[".arc"] = "application/octet-stream";
            mimeTypes[".arj"] = "application/arj";
            mimeTypes[".art"] = "image/x-jg";
            mimeTypes[".asf"] = "video/x-ms-asf";
            mimeTypes[".asm"] = "text/x-asm";
            mimeTypes[".asp"] = "text/asp";
            mimeTypes[".asx"] = "video/x-ms-asf";
            mimeTypes[".au"] = "audio/x-au";
            mimeTypes[".avi"] = "video/avi";
            mimeTypes[".bcpio"] = "application/x-bcpio";
            mimeTypes[".bin"] = "application/octet-stream";
            mimeTypes[".bm"] = "image/bmp";
            mimeTypes[".bmp"] = "image/bmp";
            mimeTypes[".boo"] = "application/book";
            mimeTypes[".book"] = "application/book";
            mimeTypes[".boz"] = "application/x-bzip2";
            mimeTypes[".bsh"] = "application/x-bsh";
            mimeTypes[".bz"] = "application/x-bzip";
            mimeTypes[".bz2"] = "application/x-bzip2";
            mimeTypes[".c"] = "text/plain";
            mimeTypes[".c++"] = "text/plain";
            mimeTypes[".cat"] = "application/vnd.ms-pki.seccat";
            mimeTypes[".cc"] = "text/plain";
            mimeTypes[".ccad"] = "application/clariscad";
            mimeTypes[".cco"] = "application/x-cocoa";
            mimeTypes[".cdf"] = "application/cdf";
            mimeTypes[".cer"] = "application/x-x509-ca-cert";
            mimeTypes[".cha"] = "application/x-chat";
            mimeTypes[".chat"] = "application/x-chat";
            mimeTypes[".class"] = "application/java";
            mimeTypes[".com"] = "application/octet-stream";
            mimeTypes[".com"] = "text/plain";
            mimeTypes[".conf"] = "text/plain";
            mimeTypes[".cpio"] = "application/x-cpio";
            mimeTypes[".cpp"] = "text/x-c";
            mimeTypes[".cpt"] = "application/x-compactpro";
            mimeTypes[".cpt"] = "application/x-cpt";
            mimeTypes[".crl"] = "application/pkcs-crl";
            mimeTypes[".crt"] = "application/x-x509-ca-cert";
            mimeTypes[".csh"] = "application/x-csh";
            mimeTypes[".css"] = "application/x-pointplus";
            mimeTypes[".css"] = "text/css";
            mimeTypes[".cxx"] = "text/plain";
            mimeTypes[".dcr"] = "application/x-director";
            mimeTypes[".deepv"] = "application/x-deepv";
            mimeTypes[".def"] = "text/plain";
            mimeTypes[".der"] = "application/x-x509-ca-cert";
            mimeTypes[".dif"] = "video/x-dv";
            mimeTypes[".dir"] = "application/x-director";
            mimeTypes[".dl"] = "video/dl";
            mimeTypes[".doc"] = "application/msword";
            mimeTypes[".dot"] = "application/msword";
            mimeTypes[".dp"] = "application/commonground";
            mimeTypes[".drw"] = "application/drafting";
            mimeTypes[".dump"] = "application/octet-stream";
            mimeTypes[".dv"] = "video/x-dv";
            mimeTypes[".dvi"] = "application/x-dvi";
            mimeTypes[".dwf"] = "model/vnd.dwf";
            mimeTypes[".dwg"] = "application/acad";
            mimeTypes[".dxf"] = "application/dxf";
            mimeTypes[".dxr"] = "application/x-director";
            mimeTypes[".el"] = "text/x-script.elisp";
            mimeTypes[".elc"] = "application/x-elc";
            mimeTypes[".env"] = "application/x-envoy";
            mimeTypes[".eps"] = "application/postscript";
            mimeTypes[".es"] = "application/x-esrehber";
            mimeTypes[".etx"] = "text/x-setext";
            mimeTypes[".evy"] = "application/envoy";
            mimeTypes[".exe"] = "application/octet-stream";
            mimeTypes[".f"] = "text/plain";
            mimeTypes[".f77"] = "text/x-fortran";
            mimeTypes[".f90"] = "text/plain";
            mimeTypes[".f90"] = "text/x-fortran";
            mimeTypes[".fdf"] = "application/vnd.fdf";
            mimeTypes[".fif"] = "application/fractals";
            mimeTypes[".fli"] = "video/fli";
            mimeTypes[".flo"] = "image/florian";
            mimeTypes[".flx"] = "text/vnd.fmi.flexstor";
            mimeTypes[".fmf"] = "video/x-atomic3d-feature";
            mimeTypes[".for"] = "text/plain";
            mimeTypes[".fpx"] = "image/vnd.fpx";
            mimeTypes[".fpx"] = "image/vnd.net-fpx";
            mimeTypes[".frl"] = "application/freeloader";
            mimeTypes[".funk"] = "audio/make";
            mimeTypes[".g"] = "text/plain";
            mimeTypes[".g3"] = "image/g3fax";
            mimeTypes[".gif"] = "image/gif";
            mimeTypes[".gl"] = "video/gl";
            mimeTypes[".gsd"] = "audio/x-gsm";
            mimeTypes[".gsm"] = "audio/x-gsm";
            mimeTypes[".gsp"] = "application/x-gsp";
            mimeTypes[".gss"] = "application/x-gss";
            mimeTypes[".gtar"] = "application/x-gtar";
            mimeTypes[".gz"] = "application/x-gzip";
            mimeTypes[".gzip"] = "application/x-gzip";
            mimeTypes[".h"] = "text/plain";
            mimeTypes[".hdf"] = "application/x-hdf";
            mimeTypes[".help"] = "application/x-helpfile";
            mimeTypes[".hgl"] = "application/vnd.hp-hpgl";
            mimeTypes[".hh"] = "text/plain";
            mimeTypes[".hlb"] = "text/x-script";
            mimeTypes[".hlp"] = "application/hlp";
            mimeTypes[".hpg"] = "application/vnd.hp-hpgl";
            mimeTypes[".hpgl"] = "application/vnd.hp-hpgl";
            mimeTypes[".hqx"] = "application/binhex";
            mimeTypes[".hta"] = "application/hta";
            mimeTypes[".htc"] = "text/x-component";
            mimeTypes[".htm"] = "text/html";
            mimeTypes[".html"] = "text/html";
            mimeTypes[".htmls"] = "text/html";
            mimeTypes[".htt"] = "text/webviewhtml";
            mimeTypes[".htx"] = "text/html";
            mimeTypes[".ice"] = "x-conference/x-cooltalk";
            mimeTypes[".ico"] = "image/x-icon";
            mimeTypes[".idc"] = "text/plain";
            mimeTypes[".ief"] = "image/ief";
            mimeTypes[".iefs"] = "image/ief";
            mimeTypes[".iges"] = "application/iges";
            mimeTypes[".igs"] = "application/iges";
            mimeTypes[".ima"] = "application/x-ima";
            mimeTypes[".imap"] = "application/x-httpd-imap";
            mimeTypes[".inf"] = "application/inf";
            mimeTypes[".ins"] = "application/x-internett-signup";
            mimeTypes[".ip"] = "application/x-ip2";
            mimeTypes[".isu"] = "video/x-isvideo";
            mimeTypes[".it"] = "audio/it";
            mimeTypes[".iv"] = "application/x-inventor";
            mimeTypes[".ivr"] = "i-world/i-vrml";
            mimeTypes[".ivy"] = "application/x-livescreen";
            mimeTypes[".jam"] = "audio/x-jam";
            mimeTypes[".jav"] = "text/plain";
            mimeTypes[".java"] = "text/plain";
            mimeTypes[".jcm"] = "application/x-java-commerce";
            mimeTypes[".jfif"] = "image/jpeg";
            mimeTypes[".jfif-tbnl"] = "image/jpeg";
            mimeTypes[".jpe"] = "image/jpeg";
            mimeTypes[".jpeg"] = "image/jpeg";
            mimeTypes[".jpg"] = "image/jpeg";
            mimeTypes[".jps"] = "image/x-jps";
            mimeTypes[".js"] = "application/x-javascript";
            mimeTypes[".jut"] = "image/jutvision";
            mimeTypes[".kar"] = "audio/midi";
            mimeTypes[".ksh"] = "application/x-ksh";
            mimeTypes[".la"] = "audio/nspaudio";
            mimeTypes[".lam"] = "audio/x-liveaudio";
            mimeTypes[".latex"] = "application/x-latex";
            mimeTypes[".lha"] = "application/octet-stream";
            mimeTypes[".lhx"] = "application/octet-stream";
            mimeTypes[".list"] = "text/plain";
            mimeTypes[".lma"] = "audio/nspaudio";
            mimeTypes[".lma"] = "audio/x-nspaudio";
            mimeTypes[".log"] = "text/plain";
            mimeTypes[".lsp"] = "application/x-lisp";
            mimeTypes[".lst"] = "text/plain";
            mimeTypes[".lsx"] = "text/x-la-asf";
            mimeTypes[".ltx"] = "application/x-latex";
            mimeTypes[".lzh"] = "application/octet-stream";
            mimeTypes[".lzx"] = "application/octet-stream";
            mimeTypes[".m"] = "text/plain";
            mimeTypes[".m"] = "text/x-m";
            mimeTypes[".m1v"] = "video/mpeg";
            mimeTypes[".m2a"] = "audio/mpeg";
            mimeTypes[".m2v"] = "video/mpeg";
            mimeTypes[".m3u"] = "audio/x-mpequrl";
            mimeTypes[".man"] = "application/x-troff-man";
            mimeTypes[".map"] = "application/x-navimap";
            mimeTypes[".mar"] = "text/plain";
            mimeTypes[".mbd"] = "application/mbedlet";
            mimeTypes[".mc$"] = "application/x-magic-cap-package-1.0";
            mimeTypes[".mcd"] = "application/mcad";
            mimeTypes[".mcf"] = "image/vasa";
            mimeTypes[".mcp"] = "application/netmc";
            mimeTypes[".me"] = "application/x-troff-me";
            mimeTypes[".mht"] = "message/rfc822";
            mimeTypes[".mhtml"] = "message/rfc822";
            mimeTypes[".mid"] = "application/x-midi";
            mimeTypes[".mid"] = "audio/midi";
            mimeTypes[".midi"] = "audio/midi";
            mimeTypes[".mif"] = "application/x-frame";
            mimeTypes[".mime"] = "www/mime";
            mimeTypes[".mjf"] = "audio/x-vnd.audioexplosion.mjuicemediafile";
            mimeTypes[".mjpg"] = "video/x-motion-jpeg";
            mimeTypes[".mm"] = "application/base64";
            mimeTypes[".mme"] = "application/base64";
            mimeTypes[".mod"] = "audio/mod";
            mimeTypes[".moov"] = "video/quicktime";
            mimeTypes[".mov"] = "video/quicktime";
            mimeTypes[".movie"] = "video/x-sgi-movie";
            mimeTypes[".mp2"] = "audio/mpeg";
            mimeTypes[".mp3"] = "audio/mpeg3";
            mimeTypes[".mpa"] = "audio/mpeg";
            mimeTypes[".mpc"] = "application/x-project";
            mimeTypes[".mpe"] = "video/mpeg";
            mimeTypes[".mpeg"] = "video/mpeg";
            mimeTypes[".mpg"] = "video/mpeg";
            mimeTypes[".mpga"] = "audio/mpeg";
            mimeTypes[".mpp"] = "application/vnd.ms-project";
            mimeTypes[".mpt"] = "application/x-project";
            mimeTypes[".mpv"] = "application/x-project";
            mimeTypes[".mpx"] = "application/x-project";
            mimeTypes[".mrc"] = "application/marc";
            mimeTypes[".ms"] = "application/x-troff-ms";
            mimeTypes[".mv"] = "video/x-sgi-movie";
            mimeTypes[".my"] = "audio/make";
            mimeTypes[".mzz"] = "application/x-vnd.audioexplosion.mzz";
            mimeTypes[".nap"] = "image/naplps";
            mimeTypes[".naplps"] = "image/naplps";
            mimeTypes[".nc"] = "application/x-netcdf";
            mimeTypes[".ncm"] = "application/vnd.nokia.configuration-message";
            mimeTypes[".nif"] = "image/x-niff";
            mimeTypes[".niff"] = "image/x-niff";
            mimeTypes[".nix"] = "application/x-mix-transfer";
            mimeTypes[".nsc"] = "application/x-conference";
            mimeTypes[".nvd"] = "application/x-navidoc";
            mimeTypes[".o"] = "application/octet-stream";
            mimeTypes[".oda"] = "application/oda";
            mimeTypes[".omc"] = "application/x-omc";
            mimeTypes[".omcd"] = "application/x-omcdatamaker";
            mimeTypes[".omcr"] = "application/x-omcregerator";
            mimeTypes[".p"] = "text/x-pascal";
            mimeTypes[".p10"] = "application/pkcs10";
            mimeTypes[".p10"] = "application/x-pkcs10";
            mimeTypes[".p12"] = "application/pkcs-12";
            mimeTypes[".p12"] = "application/x-pkcs12";
            mimeTypes[".p7a"] = "application/x-pkcs7-signature";
            mimeTypes[".p7c"] = "application/pkcs7-mime";
            mimeTypes[".p7c"] = "application/x-pkcs7-mime";
            mimeTypes[".p7m"] = "application/pkcs7-mime";
            mimeTypes[".p7m"] = "application/x-pkcs7-mime";
            mimeTypes[".p7r"] = "application/x-pkcs7-certreqresp";
            mimeTypes[".p7s"] = "application/pkcs7-signature";
            mimeTypes[".part"] = "application/pro_eng";
            mimeTypes[".pas"] = "text/pascal";
            mimeTypes[".pbm"] = "image/x-portable-bitmap";
            mimeTypes[".pcl"] = "application/vnd.hp-pcl";
            mimeTypes[".pcl"] = "application/x-pcl";
            mimeTypes[".pct"] = "image/x-pict";
            mimeTypes[".pcx"] = "image/x-pcx";
            mimeTypes[".pdb"] = "chemical/x-pdb";
            mimeTypes[".pdf"] = "application/pdf";
            mimeTypes[".pfunk"] = "audio/make";
            mimeTypes[".pfunk"] = "audio/make.my.funk";
            mimeTypes[".pgm"] = "image/x-portable-graymap";
            mimeTypes[".pgm"] = "image/x-portable-greymap";
            mimeTypes[".pic"] = "image/pict";
            mimeTypes[".pict"] = "image/pict";
            mimeTypes[".pkg"] = "application/x-newton-compatible-pkg";
            mimeTypes[".pko"] = "application/vnd.ms-pki.pko";
            mimeTypes[".pl"] = "text/plain";
            mimeTypes[".pl"] = "text/x-script.perl";
            mimeTypes[".plx"] = "application/x-pixclscript";
            mimeTypes[".pm"] = "image/x-xpixmap";
            mimeTypes[".pm"] = "text/x-script.perl-module";
            mimeTypes[".pm4"] = "application/x-pagemaker";
            mimeTypes[".pm5"] = "application/x-pagemaker";
            mimeTypes[".png"] = "image/png";
            mimeTypes[".pnm"] = "application/x-portable-anymap";
            mimeTypes[".pnm"] = "image/x-portable-anymap";
            mimeTypes[".pot"] = "application/mspowerpoint";
            mimeTypes[".pot"] = "application/vnd.ms-powerpoint";
            mimeTypes[".pov"] = "model/x-pov";
            mimeTypes[".ppa"] = "application/vnd.ms-powerpoint";
            mimeTypes[".ppm"] = "image/x-portable-pixmap";
            mimeTypes[".pps"] = "application/mspowerpoint";
            mimeTypes[".pps"] = "application/vnd.ms-powerpoint";
            mimeTypes[".ppt"] = "application/mspowerpoint";
            mimeTypes[".ppt"] = "application/powerpoint";
            mimeTypes[".ppt"] = "application/vnd.ms-powerpoint";
            mimeTypes[".ppt"] = "application/x-mspowerpoint";
            mimeTypes[".ppz"] = "application/mspowerpoint";
            mimeTypes[".pre"] = "application/x-freelance";
            mimeTypes[".prt"] = "application/pro_eng";
            mimeTypes[".ps"] = "application/postscript";
            mimeTypes[".psd"] = "application/octet-stream";
            mimeTypes[".pvu"] = "paleovu/x-pv";
            mimeTypes[".pwz"] = "application/vnd.ms-powerpoint";
            mimeTypes[".py"] = "text/x-script.phyton";
            mimeTypes[".pyc"] = "applicaiton/x-bytecode.python";
            mimeTypes[".qcp"] = "audio/vnd.qcelp";
            mimeTypes[".qd3"] = "x-world/x-3dmf";
            mimeTypes[".qd3d"] = "x-world/x-3dmf";
            mimeTypes[".qif"] = "image/x-quicktime";
            mimeTypes[".qt"] = "video/quicktime";
            mimeTypes[".qtc"] = "video/x-qtc";
            mimeTypes[".qti"] = "image/x-quicktime";
            mimeTypes[".qtif"] = "image/x-quicktime";
            mimeTypes[".ra"] = "audio/x-pn-realaudio";
            mimeTypes[".ra"] = "audio/x-pn-realaudio-plugin";
            mimeTypes[".ra"] = "audio/x-realaudio";
            mimeTypes[".ram"] = "audio/x-pn-realaudio";
            mimeTypes[".ras"] = "application/x-cmu-raster";
            mimeTypes[".ras"] = "image/cmu-raster";
            mimeTypes[".ras"] = "image/x-cmu-raster";
            mimeTypes[".rast"] = "image/cmu-raster";
            mimeTypes[".rexx"] = "text/x-script.rexx";
            mimeTypes[".rf"] = "image/vnd.rn-realflash";
            mimeTypes[".rgb"] = "image/x-rgb";
            mimeTypes[".rm"] = "application/vnd.rn-realmedia";
            mimeTypes[".rm"] = "audio/x-pn-realaudio";
            mimeTypes[".rmi"] = "audio/mid";
            mimeTypes[".rmm"] = "audio/x-pn-realaudio";
            mimeTypes[".rmp"] = "audio/x-pn-realaudio";
            mimeTypes[".rmp"] = "audio/x-pn-realaudio-plugin";
            mimeTypes[".rng"] = "application/ringing-tones";
            mimeTypes[".rng"] = "application/vnd.nokia.ringing-tone";
            mimeTypes[".rnx"] = "application/vnd.rn-realplayer";
            mimeTypes[".roff"] = "application/x-troff";
            mimeTypes[".rp"] = "image/vnd.rn-realpix";
            mimeTypes[".rpm"] = "audio/x-pn-realaudio-plugin";
            mimeTypes[".rt"] = "text/richtext";
            mimeTypes[".rt"] = "text/vnd.rn-realtext";
            mimeTypes[".rtf"] = "application/rtf";
            mimeTypes[".rtf"] = "application/x-rtf";
            mimeTypes[".rtf"] = "text/richtext";
            mimeTypes[".rtx"] = "application/rtf";
            mimeTypes[".rtx"] = "text/richtext";
            mimeTypes[".rv"] = "video/vnd.rn-realvideo";
            mimeTypes[".s"] = "text/x-asm";
            mimeTypes[".s3m"] = "audio/s3m";
            mimeTypes[".saveme"] = "application/octet-stream";
            mimeTypes[".sbk"] = "application/x-tbook";
            mimeTypes[".scm"] = "application/x-lotusscreencam";
            mimeTypes[".scm"] = "text/x-script.guile";
            mimeTypes[".scm"] = "text/x-script.scheme";
            mimeTypes[".scm"] = "video/x-scm";
            mimeTypes[".sdml"] = "text/plain";
            mimeTypes[".sdp"] = "application/sdp";
            mimeTypes[".sdp"] = "application/x-sdp";
            mimeTypes[".sdr"] = "application/sounder";
            mimeTypes[".sea"] = "application/sea";
            mimeTypes[".sea"] = "application/x-sea";
            mimeTypes[".set"] = "application/set";
            mimeTypes[".sgm"] = "text/sgml";
            mimeTypes[".sgm"] = "text/x-sgml";
            mimeTypes[".sgml"] = "text/sgml";
            mimeTypes[".sgml"] = "text/x-sgml";
            mimeTypes[".sh"] = "application/x-bsh";
            mimeTypes[".sh"] = "application/x-sh";
            mimeTypes[".sh"] = "application/x-shar";
            mimeTypes[".sh"] = "text/x-script.sh";
            mimeTypes[".shar"] = "application/x-bsh";
            mimeTypes[".shar"] = "application/x-shar";
            mimeTypes[".shtml"] = "text/html";
            mimeTypes[".shtml"] = "text/x-server-parsed-html";
            mimeTypes[".sid"] = "audio/x-psid";
            mimeTypes[".sit"] = "application/x-sit";
            mimeTypes[".sit"] = "application/x-stuffit";
            mimeTypes[".skd"] = "application/x-koan";
            mimeTypes[".skm"] = "application/x-koan";
            mimeTypes[".skp"] = "application/x-koan";
            mimeTypes[".skt"] = "application/x-koan";
            mimeTypes[".sl"] = "application/x-seelogo";
            mimeTypes[".smi"] = "application/smil";
            mimeTypes[".smil"] = "application/smil";
            mimeTypes[".snd"] = "audio/basic";
            mimeTypes[".snd"] = "audio/x-adpcm";
            mimeTypes[".sol"] = "application/solids";
            mimeTypes[".spc"] = "application/x-pkcs7-certificates";
            mimeTypes[".spc"] = "text/x-speech";
            mimeTypes[".spl"] = "application/futuresplash";
            mimeTypes[".spr"] = "application/x-sprite";
            mimeTypes[".sprite"] = "application/x-sprite";
            mimeTypes[".src"] = "application/x-wais-source";
            mimeTypes[".ssi"] = "text/x-server-parsed-html";
            mimeTypes[".ssm"] = "application/streamingmedia";
            mimeTypes[".sst"] = "application/vnd.ms-pki.certstore";
            mimeTypes[".step"] = "application/step";
            mimeTypes[".stl"] = "application/sla";
            mimeTypes[".stl"] = "application/vnd.ms-pki.stl";
            mimeTypes[".stl"] = "application/x-navistyle";
            mimeTypes[".stp"] = "application/step";
            mimeTypes[".sv4cpio"] = "application/x-sv4cpio";
            mimeTypes[".sv4crc"] = "application/x-sv4crc";
            mimeTypes[".svf"] = "image/vnd.dwg";
            mimeTypes[".svf"] = "image/x-dwg";
            mimeTypes[".svr"] = "application/x-world";
            mimeTypes[".svr"] = "x-world/x-svr";
            mimeTypes[".swf"] = "application/x-shockwave-flash";
            mimeTypes[".t"] = "application/x-troff";
            mimeTypes[".talk"] = "text/x-speech";
            mimeTypes[".tar"] = "application/x-tar";
            mimeTypes[".tbk"] = "application/toolbook";
            mimeTypes[".tbk"] = "application/x-tbook";
            mimeTypes[".tcl"] = "application/x-tcl";
            mimeTypes[".tcl"] = "text/x-script.tcl";
            mimeTypes[".tcsh"] = "text/x-script.tcsh";
            mimeTypes[".tex"] = "application/x-tex";
            mimeTypes[".texi"] = "application/x-texinfo";
            mimeTypes[".texinfo"] = "application/x-texinfo";
            mimeTypes[".text"] = "application/plain";
            mimeTypes[".text"] = "text/plain";
            mimeTypes[".tgz"] = "application/gnutar";
            mimeTypes[".tgz"] = "application/x-compressed";
            mimeTypes[".tif"] = "image/tiff";
            mimeTypes[".tif"] = "image/x-tiff";
            mimeTypes[".tiff"] = "image/tiff";
            mimeTypes[".tiff"] = "image/x-tiff";
            mimeTypes[".tr"] = "application/x-troff";
            mimeTypes[".tsi"] = "audio/tsp-audio";
            mimeTypes[".tsp"] = "application/dsptype";
            mimeTypes[".tsp"] = "audio/tsplayer";
            mimeTypes[".tsv"] = "text/tab-separated-values";
            mimeTypes[".turbot"] = "image/florian";
            mimeTypes[".txt"] = "text/plain";
            mimeTypes[".uil"] = "text/x-uil";
            mimeTypes[".uni"] = "text/uri-list";
            mimeTypes[".unis"] = "text/uri-list";
            mimeTypes[".unv"] = "application/i-deas";
            mimeTypes[".uri"] = "text/uri-list";
            mimeTypes[".uris"] = "text/uri-list";
            mimeTypes[".ustar"] = "application/x-ustar";
            mimeTypes[".ustar"] = "multipart/x-ustar";
            mimeTypes[".uu"] = "application/octet-stream";
            mimeTypes[".uu"] = "text/x-uuencode";
            mimeTypes[".uue"] = "text/x-uuencode";
            mimeTypes[".vcd"] = "application/x-cdlink";
            mimeTypes[".vcs"] = "text/x-vcalendar";
            mimeTypes[".vda"] = "application/vda";
            mimeTypes[".vdo"] = "video/vdo";
            mimeTypes[".vew"] = "application/groupwise";
            mimeTypes[".viv"] = "video/vivo";
            mimeTypes[".viv"] = "video/vnd.vivo";
            mimeTypes[".vivo"] = "video/vivo";
            mimeTypes[".vivo"] = "video/vnd.vivo";
            mimeTypes[".vmd"] = "application/vocaltec-media-desc";
            mimeTypes[".vmf"] = "application/vocaltec-media-file";
            mimeTypes[".voc"] = "audio/voc";
            mimeTypes[".voc"] = "audio/x-voc";
            mimeTypes[".vos"] = "video/vosaic";
            mimeTypes[".vox"] = "audio/voxware";
            mimeTypes[".vqe"] = "audio/x-twinvq-plugin";
            mimeTypes[".vqf"] = "audio/x-twinvq";
            mimeTypes[".vql"] = "audio/x-twinvq-plugin";
            mimeTypes[".vrml"] = "application/x-vrml";
            mimeTypes[".vrml"] = "model/vrml";
            mimeTypes[".vrml"] = "x-world/x-vrml";
            mimeTypes[".vrt"] = "x-world/x-vrt";
            mimeTypes[".vsd"] = "application/x-visio";
            mimeTypes[".vst"] = "application/x-visio";
            mimeTypes[".vsw"] = "application/x-visio";
            mimeTypes[".w60"] = "application/wordperfect6.0";
            mimeTypes[".w61"] = "application/wordperfect6.1";
            mimeTypes[".w6w"] = "application/msword";
            mimeTypes[".wav"] = "audio/wav";
            mimeTypes[".wav"] = "audio/x-wav";
            mimeTypes[".wb1"] = "application/x-qpro";
            mimeTypes[".wbmp"] = "image/vnd.wap.wbmp";
            mimeTypes[".web"] = "application/vnd.xara";
            mimeTypes[".wiz"] = "application/msword";
            mimeTypes[".wk1"] = "application/x-123";
            mimeTypes[".wmf"] = "windows/metafile";
            mimeTypes[".wml"] = "text/vnd.wap.wml";
            mimeTypes[".wmlc"] = "application/vnd.wap.wmlc";
            mimeTypes[".wmls"] = "text/vnd.wap.wmlscript";
            mimeTypes[".wmlsc"] = "application/vnd.wap.wmlscriptc";
            mimeTypes[".word"] = "application/msword";
            mimeTypes[".wp"] = "application/wordperfect";
            mimeTypes[".wp5"] = "application/wordperfect";
            mimeTypes[".wp5"] = "application/wordperfect6.0";
            mimeTypes[".wp6"] = "application/wordperfect";
            mimeTypes[".wpd"] = "application/wordperfect";
            mimeTypes[".wpd"] = "application/x-wpwin";
            mimeTypes[".wq1"] = "application/x-lotus";
            mimeTypes[".wri"] = "application/mswrite";
            mimeTypes[".wri"] = "application/x-wri";
            mimeTypes[".wrl"] = "application/x-world";
            mimeTypes[".wrl"] = "model/vrml";
            mimeTypes[".wrl"] = "x-world/x-vrml";
            mimeTypes[".wrz"] = "model/vrml";
            mimeTypes[".wrz"] = "x-world/x-vrml";
            mimeTypes[".wsc"] = "text/scriplet";
            mimeTypes[".wsrc"] = "application/x-wais-source";
            mimeTypes[".wtk"] = "application/x-wintalk";
            mimeTypes[".xbm"] = "image/x-xbitmap";
            mimeTypes[".xbm"] = "image/x-xbm";
            mimeTypes[".xbm"] = "image/xbm";
            mimeTypes[".xdr"] = "video/x-amt-demorun";
            mimeTypes[".xgz"] = "xgl/drawing";
            mimeTypes[".xif"] = "image/vnd.xiff";
            mimeTypes[".xl"] = "application/excel";
            mimeTypes[".xla"] = "application/excel";
            mimeTypes[".xla"] = "application/x-excel";
            mimeTypes[".xla"] = "application/x-msexcel";
            mimeTypes[".xlb"] = "application/excel";
            mimeTypes[".xlb"] = "application/vnd.ms-excel";
            mimeTypes[".xlb"] = "application/x-excel";
            mimeTypes[".xlc"] = "application/excel";
            mimeTypes[".xlc"] = "application/vnd.ms-excel";
            mimeTypes[".xlc"] = "application/x-excel";
            mimeTypes[".xld"] = "application/excel";
            mimeTypes[".xld"] = "application/x-excel";
            mimeTypes[".xlk"] = "application/excel";
            mimeTypes[".xlk"] = "application/x-excel";
            mimeTypes[".xll"] = "application/excel";
            mimeTypes[".xll"] = "application/vnd.ms-excel";
            mimeTypes[".xll"] = "application/x-excel";
            mimeTypes[".xlm"] = "application/excel";
            mimeTypes[".xlm"] = "application/vnd.ms-excel";
            mimeTypes[".xlm"] = "application/x-excel";
            mimeTypes[".xls"] = "application/excel";
            mimeTypes[".xls"] = "application/vnd.ms-excel";
            mimeTypes[".xls"] = "application/x-excel";
            mimeTypes[".xls"] = "application/x-msexcel";
            mimeTypes[".xlt"] = "application/excel";
            mimeTypes[".xlt"] = "application/x-excel";
            mimeTypes[".xlv"] = "application/excel";
            mimeTypes[".xlv"] = "application/x-excel";
            mimeTypes[".xlw"] = "application/excel";
            mimeTypes[".xlw"] = "application/vnd.ms-excel";
            mimeTypes[".xlw"] = "application/x-excel";
            mimeTypes[".xlw"] = "application/x-msexcel";
            mimeTypes[".xm"] = "audio/xm";
            mimeTypes[".xml"] = "application/xml";
            mimeTypes[".xml"] = "text/xml";
            mimeTypes[".xmz"] = "xgl/movie";
            mimeTypes[".xpix"] = "application/x-vnd.ls-xpix";
            mimeTypes[".xpm"] = "image/x-xpixmap";
            mimeTypes[".xpm"] = "image/xpm";
            mimeTypes[".x-png"] = "image/png";
            mimeTypes[".xsr"] = "video/x-amt-showrun";
            mimeTypes[".xwd"] = "image/x-xwd";
            mimeTypes[".xwd"] = "image/x-xwindowdump";
            mimeTypes[".xyz"] = "chemical/x-pdb";
            mimeTypes[".z"] = "application/x-compress";
            mimeTypes[".z"] = "application/x-compressed";
            mimeTypes[".zip"] = "application/x-compressed";
            mimeTypes[".zip"] = "application/x-zip-compressed";
            mimeTypes[".zip"] = "application/zip";
            mimeTypes[".zip"] = "multipart/x-zip";
            mimeTypes[".zoo"] = "application/octet-stream";
            mimeTypes[".zsh"] = "text/x-script.zsh";
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Gets a file extension and returns the corresponding mime type.
        /// </summary>
        /// <param name="fileExtension">
        /// File extension including the dot.
        /// </param>
        /// <returns>
        /// The mime type, won't be null.
        /// </returns>
        public static string GetMimeType(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                throw new ArgumentNullException("fileExtension");
            }

            string mimeType;

            if (!mimeTypes.TryGetValue(fileExtension, out mimeType))
            {
                mimeType = DefaultMimeType;
            }

            return mimeType;
        }

        #endregion Public Methods
    }
}