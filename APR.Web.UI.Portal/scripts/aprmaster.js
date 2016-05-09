//$(function () {
//    function SelectToggleMaster(s, e) { if (chkAll.GetChecked()) { aspxInvGridView.SelectAllRowsOnPage(s.GetChecked()); } else { aspxInvGridView.UnselectAllRowsOnPage(); gvClaims.PerformCallback(); } }
//    function SelectToggleChild(s, e) { if (chkAllClaims.GetChecked()) { gvClaims.SelectRows(); } else { gvClaims.UnselectRows(); } }
//    function ToggleShowScript(s, e) { aspxInvGridView.UnselectRows(); };
//    function ShowAllChildData() { aspxInvGridView.UnselectRows(); gvClaims.PerformCallback(); };

//});
//function SelectToggleChild(s, e) { if (chkAllClaims.GetChecked()) { gvClaims.SelectRows(); } else { gvClaims.UnselectRows(); } }
function OnMoreInfoClick(element, key) { var html = "<div class='popMsg'>" + key + "</div>"; popup.SetContentHtml(html); popup.ShowAtElement(element); }

$(document).ready(function () {
    AttachmetButton();
    TooltipClaimsDescription();
    //alert(dxSplitter.height);
    //if (dxSplitter.height < window.outerHeight)
    //    dxSplitter.height = window.outerHeight;
    //alert(window.outerHeight);
    $(".navMenu").menu();
    $(".navMenu").menu("option", "position", { my: "left top", at: "right-150 top+30" });
    //$("#selectAuditName").selectmenu();
    // $("#aspxInvGridView").resize(function () { $("#aspxClmGridView table").width(700); });
    //DragTable();
    //CorrectRoundPanelHeight(eval(rpnlInvGrid));
    //window.setTimeout("CorrectRoundPanelHeight", 200, eval(rpnlInvGrid));
    // $(".dxsplLCC").height("423px");
    //DataHeight=$("#aspxInvGridView_DXMainTable").height();
    //var div = $("#aspxInvGridView_DXMainTable").parent();
    //console.log(DataHeight);
    //$(div).css("height", "11px'");
    $("#aspxInvGridView_LPV").css("width","30px");
    //$('#ASPxSplitterGrid_2').parent().hide();
    // Hide the empty resize panel
    $('#ASPxSplitterGrid_1').hide();
    $('#ASPxSplitterGrid_1').height(0);
    $('#ASPxSplitterNavGrid_1').hide();
    $('#ASPxSplitterNavGrid_1').height(0);
    
    //$.ajax({
    //    type: "POST",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    url: "/FileHandler.asmx/HelloWorld",
    //    success: function (response) {
    //    }
    //});
    //ResizeGrid();
});
function ShowAttachments(invNum) {
    ShowWindowByName(invNum);
}
function ShowWindowByName(invNum) {
    var popupControl = GetPopupControl();
    var window = popupControl.GetWindowByName(name);
    popupControl.ShowWindowAtElementByID(window, "tdMenu");
}
function GetPopupControl() {
    return ASPxPopupClientControl;
}
//function CheckBoxChecked() {
//        aspxInvGridView.SelectAllRowsOnPage();
//        //gvClaims.PerformCallback();
//}
function AttachmetButton() {
    $(".printicon").button({
    }).click(function (event) {
        
        $("#download-iframe").attr("src", "/Download.ashx?printAll=" + 0 + "&InvoiceNo=" + ASPxComboBoxAuditName.GetValue());
        //event.preventDefault();
        if (window.BrowserDetect.browser !== "Explorer")
            PrintIframeIE();
        else
            PrintIEIframe();
    });
    $(".saveicon").button({
    }).on('click',function (event) {
        $("#download-iframe").attr("src", "/Download.ashx?SaveAll=" + 0 + "&InvoiceNo=" + ASPxComboBoxAuditName.GetValue());
        //event.preventDefault();
    });
    
    // $(".paperClick").button({ icons: { secondary: ".ui-icon-custom" },text:false })
}
function ResizeGrid() {
    var widht = $(".grid_12").width();
    var mainGridTd = $(".dxgvControl tbody tr td:first");
    //console.log(mainGridTd);
    var firstTR = $(".dxgvControl tbody>tr:first>td>div:nth-child(4)");
    //console.log(firstTR);
    //$("#aspxInvGridView").resizable(
    //    {
    //        alsoResize: ".dxgvTitlePanel,.dxgvGroupPanel,.dxgvFSDC,.dxgvControl tbody>tr:first>td>div:nth-child(4),.dxgvFilterBar,.dxgvTable",
    //    maxWidth: widht,
    //    minWidth: widht/2
    //    }
    //);

    //$("#aspxClmGridView").resizable({
    //    alsoResize: "#aspxClmGridView div,#aspxClmGridView table",
    //maxWidth: widht,
    //minWidth: widht / 2
    //});

    //console.log(firstTR);
    //$(firstTR).css("width", "230px");

    //$(".dvInvoiceGrid").on("resize", function (event, ui) {
    //    panelWidth = $(".dvInvoiceGrid").css("width");
    //    aspxInvGridView.SetWidth(panelWidth);
    //});
}
function OnInvoiceBeginCallBack(s, e) {
    //console.log(e);
    //if (e.command === "SORT") {con}
}
function GetSortedColumnIndex(s, e) {
    //gridviewSortedColumnIndex = e.column.index;
    //console.log(e.column);
    if (e.column.fieldName === "Attachments") e.cancel = true;
}
function LoadScripts() {
    // CheckBoxChecked();
    TooltipClaimsDescription();
    AttachmetButton();
}
function OnAllCheckedChanged(s, e) {

    if (chkAll.GetChecked()) {
        aspxInvGridView.SelectRows();
    }
    else {
        aspxInvGridView.UnselectRows();
    }
    //gvClaims.PerformCallback();
}
function OnGridSelectionChanged(s, e) {
    
    chkAll.SetChecked(s.GetSelectedRowCount() === s.cpVisibleRowCount);
   gvClaims.PerformCallback();
    
}
function TooltipClaimsDescription() {
    $('.dxgv').tooltip(
    //{
    //    position: {
    //        my: "center bottom-20",
    //        at: "center top",
    //        using: function (position, feedback) {
    //            $(this).css(position);
    //            $("<div>")
    //              .addClass("arrow")
    //              .addClass(feedback.vertical)
    //              .addClass(feedback.horizontal)
    //              .appendTo(this);
    //        }
    //    }
    //}
    );
}

function onInvoiceCellClick(rowIndex, fieldName) {
   
    //aspxInvGridView.PerformCallback(rowIndex + "|" + fieldName);
}
function OnInvoiceEditorKeyPress(editor, e) {
    if (e.htmlEvent.keyCode === 13 || e.htmlEvent.keyCode === 9) {
        aspxInvGridView.UpdateEdit();
    }
    else
        if (e.htmlEvent.keyCode === 27)
            aspxInvGridView.CancelEdit();
}
function onClaimsCellClick(rowIndex, fieldName) {
    //console.log(rowIndex);
    //console.log(fieldName);
    gvClaims.PerformCallback(rowIndex + "|" + fieldName);
}
function OnEditorKeyPress(editor, e) {
    if (e.htmlEvent.keyCode === 13 || e.htmlEvent.keyCode === 9) {
        debugger;
        gvClaims.UpdateEdit();
    }
    else
        if (e.htmlEvent.keyCode === 27)
            gvClaims.CancelEdit();
}
function CorrectRoundPanelHeight(roundPanel) {
    var mainElement = document.getElementById(roundPanel.uniqueID);
    var offsetParent = _aspxFindOffsetParent(mainElement);
    var offsetParentClearClientHeight = _aspxGetClearClientHeight(offsetParent);
    var contentCell = roundPanel.GetContentElement();
    var innerTable = _aspxGetChildByTagName(mainElement, "TABLE", 0);
    var height = contentCell.offsetHeight;
    _aspxSetOffsetHeight(contentCell, 0);
    while (innerTable.offsetHeight < offsetParentClearClientHeight)
        _aspxSetOffsetHeight(contentCell, ++height);
}

function DragTable() {
    //$(".gvResize").resizable({
    //    alsoResize: ".dxgvControl"
    //});
    //    $(".dxgvDataRow,.dxgvFocusedRow").draggable({
    //        helper: function (event, ui) {
    //            var ret = jQuery(this).clone();
    //            var width = jQuery(this)[0].offsetWidth;
    //            var myHelper = [];
    //            myHelper.push(
    //'<table style="width:' + width + 'px; background-color:green;">');
    //            myHelper.push(ret.html());
    //            myHelper.push('</table>');

    //            helper = myHelper.join('');
    //            return helper;
    //        },
    //        axis: 'y',
    //        revert: true,
    //        start: function (event, ui) {
    //            event.target.style.backgroundColor = 'blue';
    //        },
    //        stop: function (event, ui) {
    //            event.target.style.backgroundColor = 'red';
    //        }
    //    });
    //$(".dxgvDataRow td,.dxgvFocusedRow td").resizable({
    //    handles: "e",

    //    // set correct COL element and original size
    //    start: function (event, ui) {
    //        var colIndex = ui.helper.index() + 1;
    //        colElement = table.find("colgroup > col:nth-child(" +
    //        colIndex + ")");

    //        // get col width (faster than .width() on IE)
    //        colWidth = parseInt(colElement.get(0).style.width, 10);
    //        originalSize = ui.size.width;
    //    },

    //    // set COL width
    //    resize: function (event, ui) {
    //        var resizeDelta = ui.size.width - originalSize;

    //        var newColWidth = colWidth + resizeDelta;
    //        colElement.width(newColWidth);

    //        // height must be set in order to prevent IE9 to set wrong height
    //        $(this).css("height", "auto");
    //    }
    //});
    //$(".dxgvDataRow").resizable({
    //    ghost: true
    //});
}
function OnInit(s, e) {
    //ResizeGrid();
    LoadScripts();
    //AttachmetButton();
}
function OnEndCallback(s, e) {
    //if (s.cpIsUpdated) { gvClaims.PerformCallback(); }
    // AdjustSize();
    AttachmetButton();
}
//function OnControlsInitialized(s, e) {
//    ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
//        AdjustSize();
//    });
//}
//function AdjustSize() {
//    var height = Math.max(0, document.documentElement.clientHeight);
//    grid.SetHeight(height);
//}

function OnSplitterPaneResized(s, e) {
    var name = e.pane.name;
    //console.log($(gvClaims.GetRootTable()).height());
    //console.log($(aspxInvGridView.GetRootTable()).height());
    //gvClaims
    //gvInvContainer
    //gvClmsContainer
    //gvEmptyContainer
    if (name === 'gvInvContainer') {
        ResizeControl(aspxInvGridView, e.pane);
        $("#ASPxSplitterGrid").height(aspxInvGridView.GetHeight());
    } else if (name === 'gvClmsContainer') {
        
        ResizeControl(gvClaims, e.pane);
        $("#ASPxSplitterNavGrid").height(gvClaims.GetHeight());
    }
}
///Export GridData()
function ExportExcel_Click()
{
    gvClaims.PerformCallback("CSV");
    
}
//function InitialHeight() { aspxInvGridView.SetHeight(0); }
function ResizeControl(control, splitterPane) {
    
    //control.SetWidth(splitterPane.GetClientWidth());
    control.SetHeight(splitterPane.GetClientHeight());
}

function OnIndexChange(s, e) {
    var selectedAudit = document.getElementById('ctl00_aprMain_hdnSelectedAudit');
    var auditDropdown = document.getElementById('ctl00_aprMain_ASPxComboBoxAuditName');

    selectedAudit.value = auditDropdown.selectedIndex;

    aspxInvGridView.PerformCallback("OnChange");
    // aspxInvGridView.PerformCallback(s.lastSuccessValue);
    //aspxInvGridView.SelectRows();
}
function OnAuditIndexChange(s, e) {
    aspxExecutiveSummeryGridView.PerformCallback(s.lastSuccessValue);
}

function CreateAlltachmentControl(paths) {
    //Create Attachment Controls
    var html = "<div class='fileAttach'><div><input type='checkbox' id='chkfile_All' />Attachments</div><div class='attachments'>";
    if (paths!==undefined)
        $.each(paths, function (i, path) {
            //html += "<li><a onclick=\"Download('" +JSON.stringify( item) + '\'); return false;"  href="#" >File ' + (i + 1) + '</a></li>';
            html += "<div><input type='checkbox' class='chk' id='chkfile_" + i + "'/><span  title='" + path + "'  > " + GetFileNameFromPath(path) + '</span></div>';
        });
    html += "</div></div>";
    $("#AttachmentsPopControl").html(html);
    //Add event to all Checkbox
    $(".chk").change(function () {
        if (this.checked) {
            $(this).attr('checked', 'checked');
        } else {
            $(this).removeAttr('checked');
        }
        $(this).next().toggleClass("highlight");
    });
    //Check uncheck All child checkbox
    $("#chkfile_All").change(function () {
       
        var checkboxes = $(".attachments").find("input[type='checkbox']");
        var thisCheck = this.checked;
        $.each(checkboxes, function (i, chk) {
            if (thisCheck) {
                $(chk).attr('checked', 'checked');
            }
            else $(chk).removeAttr('checked');
            $(chk).next().toggleClass("highlight");
        });
    });

    $(".attachments").find("input[type='checkbox']").change(function () {
        if (checkifAllChecked()) $("#chkfile_All").attr('checked', 'checked');
        else $("#chkfile_All").removeAttr('checked');
    });
    ClientPopupControl.Show();
}
function checkifAllChecked() {
    var notall = true;
    var checkboxes = $(".attachments").find("input[type='checkbox']");
    $.each(checkboxes, function (i, chk) {
        notall = notall & this.checked;
    });

    return notall;
}
//Print Selected PDF files
function GetSelectedFilePrint(s, e) {
    var fileArray = [];
    var fileIndex = 0;
    $(".attachments").find("span.highlight").each(function () {
        fileArray.push(this.title);
    });
    //if (BrowserDetect.browser == "MSIE")
        $("#download-iframe").attr("src", "/Download.ashx?printPDFFiles=" + fileArray);
    //else
    //    document.getElementById('download-iframe').src = "/Download.ashx?printPDFFiles=" + fileArray;
       
        if (window.BrowserDetect.browser!=="Explorer")
            PrintIframeIE();
        else
            PrintIEIframe();
        //$("#download-iframe").attr("src","");
}
function PrintIEIframe() {
    if ($(window.frames["download-iframe"].document).find("pre").html()!=="") {
        window.frames['download-iframe'].focus();
        window.frames['download-iframe'].print();
        
    } else {
        setTimeout(PrintIEIframe, 1000);
    }
}

function PrintIframeIE() {

    if ($("#download-iframe").contents().find("body").html()!=="") {
        //debugger;
        //if(BrowserDetect.browser== "MSIE"){
        //    document.frames['download-iframe'].focus();
        //    document.frames['download-iframe'].print();
        //}
        //else{
            window.frames['download-iframe'].focus();
            window.frames['download-iframe'].print();
           
        //}
    } else {
        setTimeout(PrintIframe, 1000);
    }
}

//function printPage(url) {
//    var div = document.getElementById("printerDiv");
//    div.innerHTML = '<iframe src="'+url+'" onload=this.contentWindow.print();></iframe>';
//}
/// <summary>Download Selected Files</summary>
function GetSelectedFileDownLoad(s, e) {
    var fileArray = [];
    var fileIndex = 0;
    $(".attachments").find("span.highlight").each(function () {
        fileArray.push(this.title);
    });
    if (fileArray.length>1)
        $("#download-iframe").attr("src", "/Download.ashx?multipleFiles=" + fileArray);
    else
        $("#download-iframe").attr("src", "/Download.ashx?File=" + fileArray);

    //$.ajax({
    //    type: "POST",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    url: "/Download.ashx?multipleFiles="+fileArray,
    //    //data: JSON.stringify({ :  }),
    //    success: function (response) {
    //    }
    //});
}
/// <summary>Get File Name with Extension from Full File Path</summary>
/// <param name="fullPath" type="String">Full File Path</param>
/// <returns type="String" />}
function GetFileNameFromPath(fullPath)
{ return fullPath.replace(/^.*(\\|\/|\:)/, ''); }

/// <summary>Upload Files to Server</summary>
function GetFileUpload(s, e) {
}

function chkAttachCheckChanged(s, e) {
    if (s.GetValue()) $('.attachmentCheck').each(function (i, item) { $(item).attr("checked", true) });
    else { $('.attachmentCheck').each(function (i, item) { $(item).attr("checked", false) }); }
};
function checkDataItemAttachmentChecked(a, b) {  var d = aspxInvGridView.cpchkDataAttach[a - aspxInvGridView.visibleStartIndex]; console.log(d)}
function UploaderCheckChanged(s, e) { console.log(s.GetValue()); }

///Browser Detection
var BrowserDetect = {
    init: function () {
        this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
        this.version = this.searchVersion(navigator.userAgent)
			|| this.searchVersion(navigator.appVersion)
			|| "an unknown version";
        this.OS = this.searchString(this.dataOS) || "an unknown OS";
    },
    searchString: function (data) {
        for (var i = 0; i < data.length; i++) {
            var dataString = data[i].string;
            var dataProp = data[i].prop;
            this.versionSearchString = data[i].versionSearch || data[i].identity;
            if (dataString) {
                if (dataString.indexOf(data[i].subString)!==-1)
                    return data[i].identity;
            }
            else if (dataProp)
                return data[i].identity;
        }
    },
    searchVersion: function (dataString) {
        var index = dataString.indexOf(this.versionSearchString);
        if (index === -1) return;
        return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
    },
    dataBrowser: [
		{
		    string: navigator.userAgent,
		    subString: "Chrome",
		    identity: "Chrome"
		},
		{
		    string: navigator.userAgent,
		    subString: "OmniWeb",
		    versionSearch: "OmniWeb/",
		    identity: "OmniWeb"
		},
		{
		    string: navigator.vendor,
		    subString: "Apple",
		    identity: "Safari",
		    versionSearch: "Version"
		},
		{
		    prop: window.opera,
		    identity: "Opera",
		    versionSearch: "Version"
		},
		{
		    string: navigator.vendor,
		    subString: "iCab",
		    identity: "iCab"
		},
		{
		    string: navigator.vendor,
		    subString: "KDE",
		    identity: "Konqueror"
		},
		{
		    string: navigator.userAgent,
		    subString: "Firefox",
		    identity: "Firefox"
		},
		{
		    string: navigator.vendor,
		    subString: "Camino",
		    identity: "Camino"
		},
		{		// for newer Netscapes (6+)
		    string: navigator.userAgent,
		    subString: "Netscape",
		    identity: "Netscape"
		},
		{
		    string: navigator.userAgent,
		    subString: "MSIE",
		    identity: "Explorer",
		    versionSearch: "MSIE"
		},
		{
		    string: navigator.userAgent,
		    subString: "Gecko",
		    identity: "Mozilla",
		    versionSearch: "rv"
		},
		{ 		// for older Netscapes (4-)
		    string: navigator.userAgent,
		    subString: "Mozilla",
		    identity: "Netscape",
		    versionSearch: "Mozilla"
		}
    ],
    dataOS: [
		{
		    string: navigator.platform,
		    subString: "Win",
		    identity: "Windows"
		},
		{
		    string: navigator.platform,
		    subString: "Mac",
		    identity: "Mac"
		},
		{
		    string: navigator.userAgent,
		    subString: "iPhone",
		    identity: "iPhone/iPod"
		},
		{
		    string: navigator.platform,
		    subString: "Linux",
		    identity: "Linux"
		}
    ]

};
BrowserDetect.init();

//$(function () {
//    var $input = $("#fileUpload");

//    var someFunction = function () {
//        // what you actually want to do
        
//    };
//    if ($.browser.msie) {
//        // IE suspends timeouts until after the file dialog closes
//        $input.click(function (event) {
//            setTimeout(function () {
//                if ($input.val().length > 0) {
//                    someFunction();
//                }
//            }, 0);
//        });
//    }
//    else {
//        // All other browsers behave
//        $input.change(someFunction);
//    }
//    //$("#fileUpload").click(function () {
//    //    var file = this.files[0];
//    //    name = file.name;
//    //    size = file.size;
//    //    type = file.type;
//    //    console.log(file);
//    //    if (file.size > 51200000) {
//    //        alert("File is to big");
//    //    }
//    //    else if (file.type != 'image/png' && file.type != 'image/jpg' && !file.type != 'image/gif' && file.type != 'image/jpeg') {
//    //        alert("File doesn't match png, jpg or gif");
//    //    }
//    //    else {
//    //        var fd = new FormData();
//    //        fd.append("fileToUpload", file);
//    //        var xhr = new XMLHttpRequest();
//    //        xhr.upload.addEventListener("progress", uploadProgress, false);
//    //        xhr.addEventListener("load", uploadComplete, false);
//    //        xhr.addEventListener("error", uploadFailed, false);
//    //        xhr.addEventListener("abort", uploadCanceled, false);
//    //        xhr.open("POST", "/UploadHandler.ashx");
//    //        xhr.send(fd);
//    //    }
//    //});

//    //$("#fileUpload").on("change", function () {
//    //        var file = this.files[0];
//    //        name = file.name;
//    //        size = file.size;
//    //        type = file.type;
//    //        console.log(file);
//    //        if (file.size > 51200000) {
//    //            alert("File is to big");
//    //        }
//    //        else if (file.type != 'image/png' && file.type != 'image/jpg' && !file.type != 'image/gif' && file.type != 'image/jpeg') {
//    //            alert("File doesn't match png, jpg or gif");
//    //        }
//    //        else {
//    //            var fd = new FormData();
//    //            fd.append("fileToUpload", file);
//    //            var xhr = new XMLHttpRequest();
//    //            xhr.upload.addEventListener("progress", uploadProgress, false);
//    //            xhr.addEventListener("load", uploadComplete, false);
//    //            xhr.addEventListener("error", uploadFailed, false);
//    //            xhr.addEventListener("abort", uploadCanceled, false);
//    //            xhr.open("POST", "/UploadHandler.ashx");
//    //            xhr.send(fd);
//    //        }
//    //    });
//    //}
//});
//function getSize() {
//    var myFSO = new ActiveXObject("Scripting.FileSystemObject");
//    var filepath = document.fileUpload.file.value;
//    var thefile = myFSO.getFile(filepath);
//    var size = thefile.size;
//    alert(size + " bytes");
//}
////console.log(BrowserDetect);
//function ShowUploader(s, e) {
//    $("#lnkUpload").click();
//    return false;
//    $("#fileUpload").click();

//   // ClientPopupControl.Hide();
  
//    //clientCallBack.PerformCallback();
//    //$(".UploadDiv").dialog({
//    //    resizable: false,
//    //    modal: true,
//    //    buttons: {
            
//    //        Cancel: function () {
//    //            $('#UploadStatus').html("");
//    //            //$("#UploadButton").show();
//    //            $(this).dialog("close");
//    //        }
//    //    }
//    //});



//    //new AjaxUpload('#UploadButton', {
//    //    action: '/UploadHandler.ashx',
//    //    onComplete: function (file, response) {
//    //        $("<div><img src='/images/btndelete.png' onclick=\"DeleteFile('" + response + "')\"  class='delete'/>" + response + "</div>").appendTo('#UploadedFile');
//    //        $('#UploadStatus').html("file has been uploaded sucessfully");
//    //        $("#UploadButton").hide();
//    //    },
//    //    onSubmit: function (file, ext) {
//    //        if (!(ext && /^(txt|doc|docx|xls|pdf|jpg)$/i.test(ext))) {
//    //            alert('Invalid File Format.');
//    //            return false;
//    //        }
//    //        $('#UploadStatus').html("Uploading...");
//    //    }
//    //});

//}
//function uploadProgress(evt) {
//    if (evt.lengthComputable) {
//        var percentComplete = Math.round(evt.loaded * 100 / evt.total);
//        //document.getElementById('progressNumber').innerHTML = percentComplete.toString() + '%';
//        //document.getElementById('prog').value = percentComplete;
//    }
//    else {
//        //document.getElementById('progressNumber').innerHTML = 'unable to compute';
//    }
//}

//function uploadComplete(evt) {
//    /* This event is raised when the server send back a response */
//    alert(evt.target.responseText);
//}

//function uploadFailed(evt) {
//    alert("There was an error attempting to upload the file.");
//}

//function uploadCanceled(evt) {
//    alert("The upload has been canceled by the user or the browser dropped the connection.");
//}

//function DeleteFile(file) {
//    $('#UploadStatus').html("deleting...");
//    $.ajax({
//        url: "/UploadHandler.ashx?file=" + file,
//        type: "GET",
//        cache: false,
//        async: true,
//        success: function (html) {
//            $('#UploadedFile').html("");
//            $('#UploadStatus').html("file has been deleted");
//            $("#UploadButton").show();

//        }
//    });

//}
