<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
  xmlns:wix="http://wixtoolset.org/schemas/v4/wxs"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:util="http://wixtoolset.org/schemas/v4/wxs/util">
    <xsl:key name="exe-search" match="wix:Component[contains(wix:File/@Source, '.exe')]" use="@Id" />
    <xsl:template match="wix:Component[key('exe-search', @Id)]" />
    <xsl:template match="wix:ComponentRef[key('exe-search', @Id)]" />
    <xsl:template match="wix:File[contains(@Source,'\SilverMonkey.exe')]">
      <xsl:copy>
        <xsl:apply-templates select="@*|node()" />
       </xsl:copy>
    </xsl:template>
</xsl:stylesheet>