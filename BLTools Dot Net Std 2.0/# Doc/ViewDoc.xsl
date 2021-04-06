<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                exclude-result-prefixes="msxsl">
  
  <xsl:output method="html"
              indent="yes"/>

  <xsl:template match="/doc">
    <html>
      <head></head>
      <body>
        <h1>coucou</h1>
        <hr/>
        <xsl:apply-templates/>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="/doc/assembly">
    <h1>
      <xsl:value-of select="name"/>
    </h1>
  </xsl:template>

  <xsl:template match="member">
    <p>
      <xsl:value-of select="name"/>
      <xsl:apply-templates/>
    </p>
  </xsl:template>

  <xsl:template match="summary">
    <p>
      
    </p>
  </xsl:template>



</xsl:stylesheet>
