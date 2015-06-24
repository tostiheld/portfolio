var AN = AN || {};
var clicks = 0; //number of clicks (0,1 or 2)
var lastClick = [0, 0]; //last click
var cv, canvas; //canvas and context
var selectRadius = 20; //size of select
var activeVertex; //hovered vertex
var activeVertexClicked; //selected vertex
var activeEdge; //hovered edge
var activeEdgeClicked; //selected edge
var mode = "road"; //mode road|selectEdge|selectVertex
var sx; //For resizing, when the canvas is resized
var sy; //this is the multiplier so the mouse offset is fixed

//INITIALIZE CANVAS 
AN.initialize = function () {
    cv = $("#canvas")[0];
    canvas = cv.getContext("2d");
    //ADD EVENTLISTENERS
    cv.addEventListener('click', AN.handleClick, false);
    cv.addEventListener('mousemove', AN.handleMove, false);
};

//HANDLE CLICK EVENT ON CANVAS
AN.handleClick = function (e) {
    //MODE == DRAW ROADS
    if (mode == "road") {
        var curTop = findPos(cv);
        x = Math.round(AN.getCursorPosition(e)[0] * sx - curTop[0]);
        y = Math.round(AN.getCursorPosition(e)[1] * sy - curTop[1]);

        if (clicks === 0) {
            clicks++;
            if (activeVertex) {
                x = activeVertex.X;
                y = activeVertex.Y;
            }
            lastClick = [x, y];
        } else if (clicks == 1) {
            AN.drawLine(x, y, true);
        }
    //MODE == SELECT EDGE
    } else if (mode == "selectEdge") {
        //CLICK + HOVER ON VERTEX --> SELECT VERTEX
        //ELSE RESET SELECTION
        if (activeEdge) {
            activeEdgeClicked = activeEdge;
        } else {
            activeEdgeClicked = false;
        }
        //EDGE IS SELECTED SO ADD ID TO HIDDEN INPUT BOX
        //SO IT IS SUBMITTED
        if (activeEdgeClicked) {
            $(".roadConstructionEdgeId").val(activeEdgeClicked.EdgeId);
        } else {
            $(".roadConstructionEdgeId").val("");
        }
        
        //REDRAW LINES SO EDGE IS VISUALLY SELECTED
        AN.redrawLines();

    //MODE == SELECT VERTEX
    } else if (mode == "selectVertex") {
        //CLICK + HOVER ON VERTEX --> SELECT VERTEX
        //ELSE RESET SELECTION
        if (activeVertex) {
            x = activeVertex.X;
            y = activeVertex.Y;
            activeVertexClicked = activeVertex;
        }else {
            activeVertexClicked = false;
        }

        //VERTEX IS CLICKED --> ADD VERTEX ID TO HIDDEN INPUT FIELD
        //SO IT WILL BE SUBMITTED
        if (activeVertexClicked) {
            $(".schoolVertexId").val(activeVertexClicked.vertexId);
        } else {
            $(".schoolVertexId").val("");
        }
        
        //REDRAW LINES SO VERTEX IS VISUALLY SELECTED
        AN.redrawLines();
    }
};

AN.handleMove = function (e) {
    //FIND POSITION OF MOUSE
    //FIXES OFFSET + SCROLL OFFSET + SCALE OFFSET
    var curTop = findPos(cv);
    x = Math.round(AN.getCursorPosition(e)[0] * sx - curTop[0]);
    y = Math.round(AN.getCursorPosition(e)[1] * sy - curTop[1]);
    //FIND CURRENT ZONE
    var zone = window.Handler.Zones.findZoneByID($(".canvas_zoneID").val());
    //RESET ACTIVE
    var activeVertexNow = false;
    var activeEdgeNow = false;
    //WHEN MODE IS ROAD OR SELECTVERTEX CHECK 
    //IF VERTEX IS HOVERED
    if (mode == "road" || mode == "selectVertex") {
        //DRAW LINE IF CLICKED
        if (clicks == 1 && mode == "road") {
            AN.drawLine(x, y, false);
        }

        //Handle Hover of Vertexes
        $.each(zone.Edges(), function (index, value) {
            //CHECK STARTX.Y
            if (Math.sqrt((value.startX - x) * (value.startX - x) + (value.startY - y) * (value.startY - y)) < selectRadius) {
                AN.drawActiveVertex(value.startX, value.startY);
                activeVertex = {
                    X: value.startX,
                    Y: value.startY,
                    vertexId: value.StartVertexId
                };
                activeVertexNow = true;
            //CHECK ENDX.Y
            } else if (Math.sqrt((value.endX - x) * (value.endX - x) + (value.endY - y) * (value.endY - y)) < selectRadius) {
                AN.drawActiveVertex(value.endX, value.endY);
                activeVertex = {
                    X: value.endX,
                    Y: value.endY,
                    vertexId: value.EndVertexId
                };
                activeVertexNow = true;
            }
        });
    //WHEN MODE IS SELECTEDGE
    //CHECK IF EDGE IS HOVERED
    } else if (mode == "selectEdge") {

        //Handle Hover of Edge
        $.each(zone.Edges(), function (index, value) {
            if (AN.dotLineLength(x, y, value.startX, value.startY, value.endX, value.endY, true) < 10) {
                activeEdge = value;
                activeEdgeNow = true;
            }
        });

        if (activeEdgeNow) {
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

//GET CURSOR POSITION
AN.getCursorPosition = function (e) {
    var x;
    var y;

    x = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
    y = e.clientY + document.body.scrollTop + document.documentElement.scrollTop + $("#newSchoolModal").scrollTop() + $("#newRoadConstructionModal").scrollTop();

    return [x, y];
};

//GET CURSOR OFFSET + SCALE FIX
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

    return [curleft * sx, curtop * sy];
}

//DRAW LINE
//if !click -> don't refresh lines && don't save --> temporary line
//if click add edge to database
AN.drawLine = function (x, y, click) {

    if (!click) {
        AN.redrawLines();
    }
    if (activeVertex) {
        x = activeVertex.X;
        y = activeVertex.Y;
    }

    AN.drawLineNow(lastClick[0], lastClick[1], x, y);


    if (click && $(".canvas_zoneID").val() != "") {
        window.Handler.Zones.UIaddEdge([lastClick[0], lastClick[1], x, y]);
        clicks = 0;
        AN.redrawLines();
    }
};

//DRAW A LINE THAT STAYS
AN.drawLineNow = function (x1, y1, x2, y2) {
    //DON"T DRAW A LINE IF BEGIN == END
    if (x1 == x2 && y1 == y2) {
        console.log("cannot draw line to same point");
        return false;
    }
    
    //DRAW ROAD
    canvas.beginPath();
    canvas.moveTo(x1, y1);
    canvas.setLineDash([0, 0]);
    canvas.lineTo(x2, y2);
    canvas.lineWidth = 20;
    canvas.strokeStyle = '#000000';
    canvas.stroke();
    canvas.closePath();

    //DRAW DASHES
    canvas.beginPath();
    canvas.moveTo(x1, y1);
    canvas.lineTo(x2, y2);
    canvas.setLineDash([6, 10]);
    canvas.lineWidth = 3;
    canvas.strokeStyle = '#ffffff';
    canvas.stroke();
    canvas.closePath();
};


//REDRAW ALL LINES

AN.redrawLines = function () {
    canvas.clearRect(0, 0, cv.width, cv.height);
    var zone = window.Handler.Zones.findZoneByID($(".canvas_zoneID").val());

    //DRAW ROADCONSTRUCTION, ORANGE BACKGROUND
    $.each(zone.RoadConstructions(), function (index, value) {
        AN.drawRoadConstruction(value);
    });

    //DRAW EDGES
    $.each(zone.Edges(), function (index, value) {
        AN.drawLineNow(value.startX, value.startY, value.endX, value.endY);
    });
    //DRAW ACTIVE VERTEX
    if (activeVertexClicked) {
        AN.drawActiveVertex(activeVertexClicked.X, activeVertexClicked.Y);
    }
    //DRAW ACTIVE EDGE
    if (activeEdgeClicked) {
        AN.drawActiveEdge(activeEdgeClicked);
    }
};

//DRAW ROADCONSTRUCTION
//IT"S ONLY A ORANGE BACKGROUND
AN.drawRoadConstruction = function (roadConstruction) {

    var edge = window.Handler.Zones.findEdgeByID(roadConstruction.EdgeId);
    console.log(edge);

    canvas.beginPath();
    canvas.moveTo(edge.startX, edge.startY);
    canvas.setLineDash([0, 0]);
    canvas.lineTo(edge.endX, edge.endY);
    canvas.lineWidth = 28;
    canvas.strokeStyle = '#FFA500';
    canvas.stroke();
    canvas.closePath();

};

//DRAW SELECTED/HOVERED VERTEX
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

//DRAW SELECTED/HOVERED EDGE
AN.drawActiveEdge = function (edge) {
    console.log("edge");
    console.log(edge);
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
AN.dotLineLength = function (x, y, x0, y0, x1, y1, o) {
    function lineLength(x, y, x0, y0) {
        return Math.sqrt((x -= x0) * x + (y -= y0) * y);
    }
    if (o && !(o = function (x, y, x0, y0, x1, y1) {
            if ((x1 - x0) === 0) return {
                x: x0,
                y: y
            };
            else if ((y1 - y0) === 0) return {
                x: x,
                y: y0
            };
            var left, tg = -1 / ((y1 - y0) / (x1 - x0));
            return {
                x: left = (x1 * (x * tg - y + y0) + x0 * (x * -tg + y - y1)) / (tg * (x1 - x0) + y0 - y1),
                y: tg * left - tg * x + y
            };
        }(x, y, x0, y0, x1, y1), o.x >= Math.min(x0, x1) && o.x <= Math.max(x0, x1) && o.y >= Math.min(y0, y1) && o.y <= Math.max(y0, y1))) {
        var l1 = lineLength(x, y, x0, y0),
            l2 = lineLength(x, y, x1, y1);
        return l1 > l2 ? l2 : l1;
    } else {
        var a = y0 - y1,
            b = x1 - x0,
            c = x0 * y1 - y0 * x1;
        return Math.abs(a * x + b * y + c) / Math.sqrt(a * a + b * b);
    }
};

