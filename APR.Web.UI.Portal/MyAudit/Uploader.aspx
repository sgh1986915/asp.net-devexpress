<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Uploader.aspx.cs" Inherits="APR.Web.UI.Portal.MyAudit.Uploader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../../js/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" media="screen" />

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
<script type="text/javascript" src="http://bp.yahooapis.com/2.4.21/browserplus-min.js"></script>

<script type="text/javascript" src="../../js/plupload.js"></script>
<script type="text/javascript" src="../../js/plupload.gears.js"></script>
<script type="text/javascript" src="../../js/plupload.silverlight.js"></script>
<script type="text/javascript" src="../../js/plupload.flash.js"></script>
<script type="text/javascript" src="../../js/plupload.browserplus.js"></script>
<script type="text/javascript" src="../../js/plupload.html4.js"></script>
<script type="text/javascript" src="../../js/plupload.html5.js"></script>
<script type="text/javascript" src="../../js/jquery.plupload.queue/jquery.plupload.queue.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>



<form method="post" action="../dump.php">

	<div style="float: left; margin-right: 20px">
		<h3>Flash runtime</h3>
		<div id="flash_uploader" style="width: 450px; height: 330px;">You browser doesn't have Flash installed.</div>

		<h3>Gears runtime</h3>
		<div id="gears_uploader" style="width: 450px; height: 330px;">You browser doesn't have Gears installed.</div>
	</div>

	<div style="float: left; margin-right: 20px">
		<h3>Silverlight runtime</h3>
		<div id="silverlight_uploader" style="width: 450px; height: 330px;">You browser doesn't have Silverlight installed.</div>

		<h3>HTML 5 runtime</h3>
		<div id="html5_uploader" style="width: 450px; height: 330px;">You browser doesn't support native upload. Try Firefox 3 or Safari 4.</div>
	</div>

	<div style="float: left; margin-right: 20px">
		<h3>BrowserPlus runtime</h3>
		<div id="browserplus_uploader" style="width: 450px; height: 330px;">You browser doesn't have BrowserPlus installed.</div>

		<h3>HTML 4 runtime</h3>
		<div id="html4_uploader" style="width: 450px; height: 330px;">You browser doesn't have HTML 4 support.</div>
	</div>

	<br style="clear: both" />

	<input type="submit" value="Send" />
</form>
    </div>
    </form>
</body>
</html>
