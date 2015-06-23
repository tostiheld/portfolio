var AN = AN || {};
var clicks = 0;
var lastClick = [0, 0];
var cv, canvas;
var lines = [];
var selectRadius = 20;
var activeVertex;
var activeEdge;
var mode = "road";
var sx;
var sy;

AN.initialize = function () {
    cv = $("#canvas")[0];
    canvas = cv.getContext("2d");
    cv.addEventListener('click', AN.handleClick, false);
    cv.addEventListener('mousemove', AN.handleMove, false);
};

AN.handleClick = function (e) {
    if (mode == "road") {
        var curTop = findPos(cv);
        //fix scroll
        x = Math.round(AN.getCursorPosition(e)[0]*sx - curTop[0]);
        y = Math.round(AN.getCursorPosition(e)[1]*sy - curTop[1]);

        if (clicks === 0) {
            clicks++;
            if (activeVertex) {
                x = activeVertex[0];
                y = activeVertex[1];
            }
            lastClick = [x, y];
        } else if (clicks == 1) {
            AN.drawLine(x, y, true);
        }
    }else if(mode == "selectEdge"){
        if(activeEdge){
            cv.attr("edge",activeEdge.id);
        }else {
            cv.attr("edge","");
        }
    }
};

AN.handleMove = function (e) {
    var curTop = findPos(cv);
    x = Math.round(AN.getCursorPosition(e)[0]*sx - curTop[0]);
    y = Math.round(AN.getCursorPosition(e)[1]*sy - curTop[1]);
    var zone = window.Handler.Zones.findZoneByID($(".canvas_zoneID").val());
    var activeVertexNow = false;
    var activeEdgeNow = false;
    if(mode == "road"){
        if (clicks == 1) {
            AN.drawLine(x, y, false);
        }

        //Handle Hover of Vertex
        $.each(zone.Edges(), function (index, value) {
            if (Math.sqrt((value.startX - x) * (value.startX - x) + (value.startY - y) * (value.startY - y)) < selectRadius) {
                AN.drawActiveVertex(value.startX, value.startY);
                activeVertex = [value.startX, value.startY];
                activeVertexNow = true;
            } else if (Math.sqrt((value.endX - x) * (value.endX - x) + (value.endY - y) * (value.endY - y)) < selectRadius) {
                AN.drawActiveVertex(value.endX, value.endY);
                activeVertex = [value.endX, value.endY];
                activeVertexNow = true;
            }
        }); 
    }else if(mode == "select"){
    
        //Handle Hover of Edge
        $.each(zone.Edges(), function (index, value) {
            if(AN.dotLineLength(x, y, value.startX, value.startY, value.endX, value.endY, true)<10){
                activeEdge = value;
                activeEdgeNow = true;
            }
        });
        
        if(activeEdgeNow){
            AN.drawActiveEdge(activeEdge);
        }


    }
    if (!activeVertexNow && activeVertex) {
        AN.redrawLines();
        activeVertex = false;
    }

    if (!activeEdgeNow && activeEdge) {
        AN.redrawLines();
        activeEdge = false;
    }
    
};

AN.getCursorPosition = function (e) {
    var x;
    var y;

    if (e.pageX !== undefined && e.pageY !== undefined) {
        x = e.pageX;
        y = e.pageY;
    } else {
        x = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
        y = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
    }

    return [x, y];
};

AN.drawLine = function (x, y, click) {

    if (!click) {
        AN.redrawLines();
    }
    if (activeVertex) {
        x = activeVertex[0];
        y = activeVertex[1];
    }

    AN.drawLineNow(lastClick[0], lastClick[1], x, y);


    if (click) {
        window.Handler.Zones.UIaddEdge([lastClick[0], lastClick[1], x, y]);
        clicks = 0;
        AN.redrawLines();
    }
};

AN.drawLineNow = function (x1, y1, x2, y2) {
    if (x1 == x2 && y1 == y2) {
        console.log("cannot draw line to same point");
        return false;
    }
    canvas.beginPath();
    canvas.moveTo(x1, y1);
    canvas.setLineDash([0, 0]);
    canvas.lineTo(x2, y2);
    canvas.lineWidth = 20;
    canvas.strokeStyle = '#000000';
    canvas.stroke();
    canvas.closePath();

    canvas.beginPath();
    canvas.moveTo(x1, y1);
    canvas.lineTo(x2, y2);
    canvas.setLineDash([6, 10]);
    canvas.lineWidth = 3;
    canvas.strokeStyle = '#ffffff';
    canvas.stroke();
    canvas.closePath();
    

};

AN.redrawLines = function () {
    canvas.clearRect(0, 0, cv.width, cv.height);
    var zone = window.Handler.Zones.findZoneByID($(".canvas_zoneID").val());
    $.each(zone.Edges(), function (index, value) {
        AN.drawLineNow(value.startX, value.startY, value.endX, value.endY);
    });
};

AN.drawActiveVertex = function (x, y) {
    canvas.beginPath();
    canvas.arc(x, y, selectRadius, 0, 2 * Math.PI, false);
    canvas.fillStyle = 'transparent';
    canvas.fill();
    canvas.lineWidth = 2;
    canvas.strokeStyle = '#E82C0C';
    canvas.stroke();
    canvas.closePath();
};
AN.drawActiveEdge = function (edge) {
    canvas.beginPath();
    canvas.moveTo(edge.startX, edge.startY);
    canvas.setLineDash([0, 0]);
    canvas.lineTo(edge.endX, edge.endY);
    canvas.lineWidth = 20;
    canvas.strokeStyle = '#222222';
    canvas.stroke();
    canvas.closePath();

    canvas.beginPath();
    canvas.moveTo(edge.startX, edge.startY);
    canvas.lineTo(edge.endX, edge.endY);
    canvas.setLineDash([6, 10]);
    canvas.lineWidth = 3;
    canvas.strokeStyle = '#E82C0C';
    canvas.stroke();
    canvas.closePath();
};


/**
 * See: http://jsfromhell.com/math/dot-line-length
 *
 * Distance from a point to a line or segment.
 *
 * @param {number} x point's x coord
 * @param {number} y point's y coord
 * @param {number} x0 x coord of the line's A point
 * @param {number} y0 y coord of the line's A point
 * @param {number} x1 x coord of the line's B point
 * @param {number} y1 y coord of the line's B point
 * @param {boolean} overLine specifies if the distance should respect the limits
 * of the segment (overLine = true) or if it should consider the segment as an
 * infinite line (overLine = false), if false returns the distance from the point to
 * the line, otherwise the distance from the point to the segment.
 */
AN.dotLineLength = function(x, y, x0, y0, x1, y1, o) {
  function lineLength(x, y, x0, y0){
    return Math.sqrt((x -= x0) * x + (y -= y0) * y);
  }
  if(o && !(o = function(x, y, x0, y0, x1, y1){
    if((x1 - x0)===0) return {x: x0, y: y};
    else if((y1 - y0)===0) return {x: x, y: y0};
    var left, tg = -1 / ((y1 - y0) / (x1 - x0));
    return {x: left = (x1 * (x * tg - y + y0) + x0 * (x * - tg + y - y1)) / (tg * (x1 - x0) + y0 - y1), y: tg * left - tg * x + y};
  }(x, y, x0, y0, x1, y1), o.x >= Math.min(x0, x1) && o.x <= Math.max(x0, x1) && o.y >= Math.min(y0, y1) && o.y <= Math.max(y0, y1))){
    var l1 = lineLength(x, y, x0, y0), l2 = lineLength(x, y, x1, y1);
    return l1 > l2 ? l2 : l1;
  }
  else {
    var a = y0 - y1, b = x1 - x0, c = x0 * y1 - y0 * x1;
    return Math.abs(a * x + b * y + c) / Math.sqrt(a * a + b * b);
  }
};

function findPos(obj) {
	var curleft = curtop = 0;
	if (obj.offsetParent) {
	do {
			curleft += obj.offsetLeft;
			curtop += obj.offsetTop;

		} while (obj = obj.offsetParent);
    }
    
    sx = canvas.canvas.width / canvas.canvas.offsetWidth;
    sy = canvas.canvas.height / canvas.canvas.offsetHeight;
    
	return [curleft*sx,curtop*sy];
}