$(function() {
    AttachUploader();
    $("input#lnkPrint").click(function() {
        GetFiles($("tr.dxgvFocusedRow td:eq(1)").text(), ASPxComboBoxAuditName.GetValue(), 'print');
    });
    $("input#lnkDownload,input#lnkOpen").click(function() {
        GetFiles($("tr.dxgvFocusedRow td:eq(1)").text(), ASPxComboBoxAuditName.GetValue(), 'save');
    });
});
function GetFiles(invoice, folder, model) {
    var files = '';
    $("div.attachments > div:has(input:checkbox:checked) span").each(function () { files += $.trim($(this).text()) + ','; });

    if (files === '') {
        alert('Please select file(s)');
        return;
    }
    $("#download-iframe").attr("src", "/printhandler.ashx?invoice=" + invoice + "&folder=" + folder + "&model=" + model + "&files=" + files);
}
function AttachUploader() {
    var uploader = new plupload.Uploader({
        runtimes: 'html5,browserplus,gears,flash,silverlight,html4',
        browse_button: 'lnkUpload',
        container: 'container',
        max_file_size: '10mb',
        dragdrop: true,
        drop_element: "container",
        url: '/uploadhandler.ashx',
        flash_swf_url: '../plupload/js/plupload.flash.swf',
        silverlight_xap_url: '../plupload/js/plupload.silverlight.xap',
        filters: [
            {
                title: "Image files",
                extensions: "jpg,gif,png"
            }
        ]
    });
    uploader.bind('Init', function (up, params) {
        //$('#filelist').html("<div>Current runtime: " + params.runtime + "</div>");
    });
    $('#uploadfiles').click(function (e) {
        uploader.start();
        e.preventDefault();
    });
    uploader.init();
    uploader.bind('FilesAdded', function (up, files) {
        $.each(files, function (i, file) {
            $('#filelist').append('<div id="' + file.id + '">' + file.name + ' (' + plupload.formatSize(file.size) + ') <b></b>' + '</div>');
        });
        up.refresh()// Reposition Flash/Silverlight
        ;
        uploader.start();
    });
    uploader.bind('UploadProgress', function (up, file) {
        $('#' + file.id + " b").html(file.percent + "%");
    });
    uploader.bind('Error', function (up, err) {
        $('#filelist').append("<div>Error: " + err.code + ", Message: " + err.message + (err.file ? ", File: " + err.file.name : "") + "</div>");
        up.refresh()// Reposition Flash/Silverlight
        ;
    });
    uploader.bind('FileUploaded', function (up, file, response) {
        //$('#filelist').empty();
        var filePath = response.response;
        //$("#LogoUrl").val(filePath);
        //$("#imgLogo").attr("src", filePath);
        //ClientPopupControl.RefreshContentUrl()
        //var window = ClientPopupControl.GetWindowByName('PopupControlContentControl');
        //ClientPopupControl.RefreshWindowContentUrl(window);
        //ClientPopupControl.ShowWindow(window);
        ClientPopupControl.Hide();
        aspxInvGridView.PerformCallback();
        alert("Your file has been uploaded successfully with the name " + filePath);
        
        //$('.fileAttach').fadeOut('slow', function () {  }).fadeIn("5000");
    });
    uploader.bind('BeforeUpload', function (up, file) {

        ////up.settings.url += "?desc=" + curFile.Description;
        //// You can override settings before the file is uploaded
        //// up.settings.url = 'upload.php?id=' + file.id;
        //var id = fileManager.ObjectId == '00000000-0000-0000-0000-000000000000' ? fileManager.hdnTempId : fileManager.ObjectId;

        up.settings.multipart_params = { folder: ASPxComboBoxAuditName.GetValue(), invoice: $("tr.dxgvFocusedRow td:eq(1)").text() };
    });
}
function printTrigger(mode) {
    if (mode == "save") return;
    var getMyFrame = document.getElementById('download-iframe');
    getMyFrame.focus();
    getMyFrame.contentWindow.print();
}