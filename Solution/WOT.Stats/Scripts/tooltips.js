var agt = navigator.userAgent.toLowerCase();
var is_major = parseInt(navigator.appVersion);
var is_minor = parseFloat(navigator.appVersion);

var is_nav = ((agt.indexOf('mozilla') != -1) && (agt.indexOf('spoofer') == -1)
                   && (agt.indexOf('compatible') == -1) && (agt.indexOf('opera') == -1)
                   && (agt.indexOf('webtv') == -1) && (agt.indexOf('hotjava') == -1));
var is_nav4 = (is_nav && (is_major == 4));
var is_nav6 = (is_nav && (is_major == 5));
var is_nav6up = (is_nav && (is_major >= 5));
var is_ie = ((agt.indexOf("msie") != -1) && (agt.indexOf("opera") == -1));
//tooltip Position
var offsetX = 0;
var offsetY = 5;
var opacity = 100;
var toolTipSTYLE;
var iwidth = 0;
var ileft = 0;

function initToolTips() {
    if (document.getElementById) {
        toolTipSTYLE = document.getElementById("toolTipLayer").style;
    }
    if (is_ie || is_nav6up) {
        toolTipSTYLE.visibility = "visible";
        toolTipSTYLE.display = "none";
        document.onmousemove = moveToMousePos;
    }
}

function moveToMousePos(e) {

    if (!is_ie) {
        x = e.pageX;
        y = e.pageY;
    } else {
        x = event.x + document.body.scrollLeft + document.documentElement.scrollLeft;
        y = event.y + document.body.scrollTop + document.documentElement.scrollTop;
    }

    var iclientHeigth = 0
    var iclientWidth = 0
    try {
        iclientWidth = document.getElementById("toolTipLayer").childNodes[0].offsetWidth;
        if ((x + offsetX + (iclientWidth)) > document.documentElement.offsetWidth)
            toolTipSTYLE.left = ((x + offsetX) - (iclientWidth)) + 'px';
        else
            toolTipSTYLE.left = (x + (offsetX)) + 'px';

        iclientHeigth = document.getElementById("toolTipLayer").childNodes[0].offsetHeight;
        if ((y + offsetY + (iclientHeigth)) > document.documentElement.offsetHeight)
            toolTipSTYLE.top = ((y + offsetY) - iclientHeigth) + 'px';
        else
            toolTipSTYLE.top = (y + offsetY) + 'px';

        if (iclientHeigth == 0 || iclientWidth == 0)
            toolTipSTYLE.display = "none";

        return true;
    }
    catch (ex) {
        return true;
    }
}




function ShowEffValues(arrayString) {
    try {
        var rows = arrayString.split(";");
        var s = '<table width="100%" cellspacing="0" cellpadding="5" border="0">';
        for (var r = 0; r < rows.length; r++) {
            var cells = rows[r].split("|");

            if (cells[6].toUpperCase() === "H") {
                s += '<tr><td width = "33.34%">' + cells[0] + '</td><td width = "33.33%" align="right"><strong>' + cells[1] + '</strong></td><td width = "33.33%" align="right" style="white-space:nowrap;"><strong>' + cells[2] + '</strong></td></tr>';
            }
            else if (cells[6].toUpperCase() === "D") {
                s += '<tr><td align="left">' + cells[0] + '</td><td align="right" style="white-space:nowrap;">&nbsp;' + cells[1] + '</td><td align="right" style="white-space:nowrap;">';
                s += '<table cellspacing="0" cellpadding="0" border="0"><tr><td bgcolor="#999999" width="' + cells[3] + '%"></td><td bgcolor="#00FF00" width="' + cells[4] + '"></td></tr></table>';
                s += '&nbsp;' + cells[2] + '</td></tr>';
            }
            else if (cells[6].toUpperCase() === "T") {
                s += '<tr><td align="left" colspan="2"><strong>Total</strong></td><td align="right"><strong> &nbsp;' + cells[2] + '</strong></td></tr>';
            }

        }
        s += '</table>';
        offsetX = -220;

        toolTip(s);
    } catch (e) {
        tooltip();

    }
}

function ShowWN8Values(arrayString) {
    try {
        var rows = arrayString.split(";");
        var s = '<table width="100%" cellspacing="0" cellpadding="5" border="0">';
        for (var r = 0; r < rows.length; r++) {
            var cells = rows[r].split("|");

            if (cells[7].toUpperCase() === "H") {
                s += '<tr><td width = "25%">' + cells[0] + '</td><td width = "25%" align="right"><strong>' + cells[1] + '</strong></td><td width = "25%" align="right" style="white-space:nowrap;"><strong>' + cells[2] + '</strong></td><td width = "25%" align="right" style="white-space:nowrap;"><strong>' + cells[3] + '</strong></td></tr>';
            }
            else if (cells[7].toUpperCase() === "D") {
                s += '<tr><td align="left">' + cells[0] + '</td><td align="right" style="white-space:nowrap;">&nbsp;' + cells[1] + '</td><td align="right">&nbsp;' + cells[3] + '</td><td align="right" style="white-space:nowrap;">';
                s += '<table cellspacing="0" cellpadding="0" border="0"><tr><td bgcolor="#999999" width="' + cells[4] + '%"></td><td bgcolor="#00FF00" width="' + cells[5] + '"></td></tr></table>';
                s += '&nbsp;' + cells[2] + '</td></tr>';
            }
            else if (cells[7].toUpperCase() === "T") {
                s += '<tr><td align="left" colspan="2"><strong>Total</strong></td><td align="right">&nbsp;</td><td align="right"><strong> &nbsp;' + cells[2] + '</strong></td></tr>';
            }

        }
        s += '</table>';
        offsetX = 0;

        toolTip(s);
    } catch (e) {
        tooltip();

    }
}

function ShowShotsHits(colid, hitsCaption, hits, shotsCaption, shots) {
    s = '<table width="100%" cellspacing="0" cellpadding="5" border="0">';
    s += '<tr><td align="right">' + shotsCaption + ': </td><td align="right">&nbsp;' + numberFormatter(shots, 0) + '</td></tr>';
    s += '<tr><td align="right">' + hitsCaption + ': </td><td align="right">&nbsp;' + numberFormatter(hits, 0) + '</td></tr>';
    s += '</table>';

    offsetX = -120;
    toolTip(s);
}

function fragList(frags) {
    s = '<table class="l-div-content b-gray-text" width="100%" cellspacing="0" cellpadding="5" border="0">';
    var fullList = frags.split(";")
    for (var i = 0; i < fullList.length && i < 15; i++) {

        var myString = fullList[i].split("|")
        s += '<tr><td class="' + myString[0] + ' td-armory-icon">';
        s += '<div class="wrapper">';
        s += '<span class="level">';
        s += '<a class="b-gray-text">' + myString[1] + '</a>';
        s += '</span>';
        s += '<a><Image src= "' + myString[2] + '"/></a></div></td>';
        s += '<td bgcolor="#FFFFCC" style="white-space:nowrap;"><font face="Arial,Helvetica,sans-serif" color="black">' + myString[3] + '</font></td></tr>';
    }

    s += '</table>';

    offsetX = 0;
    toolTip(s);
}

function thousandSeparator(n, sep) {
    var sRegExp = new RegExp('(-?[0-9]+)([0-9]{3})'),
		sValue = n + '';

    if (sep === undefined) { sep = ','; }
    while (sRegExp.test(sValue)) {
        sValue = sValue.replace(sRegExp, '$1' + sep + '$2');
    }
    return sValue;
}

function numberFormatter(nmbr, decimals) {
    nmbr = nmbr.toString(); // when 0 value is passed to following replace line, firefox crashes. we need to pass '0'
    nmbr = nmbr.replace(',', "");
    if (!isFinite(nmbr)) {
        return nmbr;
    }


    nmbr = Number(nmbr);
    nmbr = nmbr.toFixed(decimals);
    nmbr += '';
    x = nmbr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}


function toolTip(msg, fg, bg) {
    try {
        if (toolTip.arguments.length < 1) // if no arguments are passed then hide the tootip
        {
            if (is_nav4)
                toolTipSTYLE.visibility = "hidden";
            else
                toolTipSTYLE.display = "none";
        }
        else // show
        {
            if (!fg) fg = "black";
            if (!bg) bg = "#FFFFCC";
            var content = '<table id="tooltipEff" border="0" cellspacing="0" cellpadding="5" width="100px"  class="toolTip"><tr><td bgcolor="' + fg + '">' +
                                     '<table border="0" cellspacing="0" cellpadding="5"<tr><td bgcolor="' + bg + '">' +
                                     '<font face="Arial, Helvetica, sans-serif" color="' + fg + '" size="-2">' + msg +
                                     '</font></td></tr></table>' +
                                     '</td></tr></table>';
            if (is_nav4) {
                toolTipSTYLE.document.write(content);
                toolTipSTYLE.document.close();
                toolTipSTYLE.visibility = "visible";
            }

            else if (is_ie || is_nav6up) {
                document.getElementById("toolTipLayer").innerHTML = content;
                toolTipSTYLE.display = 'block'
            }
        }
    } catch (e) {

    }
} 