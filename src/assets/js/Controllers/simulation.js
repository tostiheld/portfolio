var AN = AN || {};
var clicks = 0;
var lastClick = [0, 0];
var cv, canvas;
var lines = [];
var selectRadius = 20;
var active;
var modus = 'road';

AN.initialize = function () {
    cv = $("#canvas")[0];
    canvas = cv.getContext("2d");
    cv.addEventListener('click', AN.handleClick, false);
    cv.addEventListener('mousemove', AN.handleMove, false);

};

AN.handleClick = function (e) {
    if (modus == "road") {
        x = AN.getCursorPosition(e)[0] - this.offsetLeft;
        y = AN.getCursorPosition(e)[1] - this.offsetTop;

        if (clicks === 0) {
            clicks++;
            if (active) {
                x = active[0];
                y = active[1];
            }
            lastClick = [x, y];
        } else if (clicks == 1) {
            AN.drawLine(x, y, true);
        }
    }
};

AN.handleMove = function (e) {
    x = AN.getCursorPosition(e)[0] - this.offsetLeft;
    y = AN.getCursorPosition(e)[1] - this.offsetTop;

    if (clicks == 1) {
        AN.drawLine(x, y, false);
    }
    var activeNow = false;
    $.each(lines, function (index, value) {
        if (Math.sqrt((value[0] - x) * (value[0] - x) + (value[1] - y) * (value[1] - y)) < selectRadius) {
            AN.drawActive(value[0], value[1]);
            active = [value[0], value[1]];
            activeNow = true;
        } else if (Math.sqrt((value[2] - x) * (value[2] - x) + (value[3] - y) * (value[3] - y)) < selectRadius) {
            AN.drawActive(value[2], value[3]);
            active = [value[2], value[3]];
            activeNow = true;
        }
    });
    if (!activeNow && active) {
        AN.redrawLines();
        active = false;

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
    if (active) {
        x = active[0];
        y = active[1];
    }

    AN.drawLineNow(lastClick[0], lastClick[1], x, y);


    if (click) {
        lines.push([lastClick[0], lastClick[1], x, y]);
        window.Handler.Zones.UIaddEdge([lastClick[0], lastClick[1], x, y]);
        clicks = 0;
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
    $.each(lines, function (index, value) {
        AN.drawLineNow(value[0], value[1], value[2], value[3]);
    });
};

AN.drawActive = function (x, y) {
    canvas.beginPath();
    canvas.arc(x, y, selectRadius, 0, 2 * Math.PI, false);
    canvas.fillStyle = 'transparent';
    canvas.fill();
    canvas.lineWidth = 2;
    canvas.strokeStyle = '#E82C0C';
    canvas.stroke();
    canvas.closePath();
};