let canvasClasses = new Map();
export function initCanvas(canvasId,imageId) {
    var canvas = canvasClasses.get(canvasId);
    if (canvas === undefined) {
        var canvas = canvas = document.getElementById(canvasId);
        if (canvas === null)
            return;
        canvasClasses.set(canvasId, new CanvasClass(canvasId, imageId));
    }
    return "initCanvas";
}
export function OnDrawPreview(int x, int y) {
    var canvas = canvasClasses.get(canvasId);
    if (canvas != undefined) {
        var canvas = canvasClasses.get(canvasId);
        if (canvas === null)
            return;

    }
    return "OnDrawPreview";
}

export function CanvasRedraw() {
    /* canvasClasses.forEach(
       (value,key) => { value.onCanvasSizeChange}
    ) */

    for (let value of canvasClasses.values()) {
        value.onCanvasSizeChange()
    }
    //canvasClasses.get('can').onCanvasSizeChange();
    return "CanvasRedraw";
}
export function OnClearDraw(canvasId) {
    var id = canvasClasses.get(canvasId);
    if (id) {
        id.clearDraw();
    }
    return "OnClearDraw";
}
export function OnNewline(canvasId) {
    var id = canvasClasses.get(canvasId);
    if (id) {
        id.SetNewLine();
    }
    first = true;
    return "OnNewline";
}
export function OnUnDo(canvasId) {
    var id = canvasClasses.get(canvasId);
    if (id) {
        id.unDo();
    }
    return "OnUnDo";
}
export function mouseDraw(canvasId) {
    var id = canvasClasses.get(canvasId);
    if (id) {
        id.newLine = false;
        id.bDrawOnMove = true;
    }
    return "mouseDraw";
}
export function setCanvasImage(canvasId, imageId) {
    document.getElementById(imageId).src = imagesrc;
    var id = canvasClasses.get(canvasId);
    if (id != null) {
        id.setImage(imageId);
    }
    return "setCanvasImage";
}

class CanvasClass {
    canvas;
    img;
    canvasId;
    imageId;
    ctx;
    prevX;
    currX;
    prevY;
    currY;
    bDrawOnMove = false;
    BRectLeft;
    BRectTop;
    scaleMode = 2;
    pathsarray = [];
    points = [];
    newLine = true;
    lastDraw = { x: -1, y: -1 };
    cuurentStrokStyle = 'red';
    constructor(canvasId, imageId) {
        this.canvasId = canvasId;
        this.imageId = imageId;
        this.canvas = document.getElementById(canvasId);
        this.setImage(this.imageId);
        this.ctx = this.canvas.getContext('2d');
        this.ctx.strokeStyle = this.cuurentStrokStyle;
        this.GetCanvasBoundingRect();
        this.canvas.width = window.innerWidth;
        this.canvas.height = window.innerHeight;
        this.canvas.addEventListener(
            'mousemove',
            (e) => {
                this.move(e);
            },
            false
        );
        this.canvas.addEventListener(
            'mousedown',
            (e) => {
                this.onMouseDown(e);
            },
            false
        );
        this.canvas.addEventListener(
            'mouseup',
            (e) => {
                this.bDrawOnMove = false;
            },
            false
        );


    }
    unDo() {
        this.pathsarray.pop();
        this.drawPath();
    }
    onCanvasSizeChange() {

        this.windowResize();
    }
    move(e) {
        if (this.bDrawOnMove) {
            this.prevX = this.currX;
            this.prevY = this.currY;
            this.currX = e.clientX - this.canvas.offsetLeft;
            this.currY = e.clientY - this.canvas.offsetTop;
            this.draw();
        }
    }
    onMouseDown(e) {
        if (this.bDrawOnMove === true) {
        }
        else {
            if (this.newLine === true) {
                this.newLine = false;
                this.prevX = e.clientX;
                this.prevY = e.clientY;
                return;
            }
            this.draw(e.clientX, e.clientY);
        }
    }
    windowResize() {
        this.canvas.width = window.innerWidth;
        this.canvas.height = window.innerHeight;
        this.GetCanvasBoundingRect();
        /* erase(false);*/
        this.scaleToFit(this.img);
        this.drawPath();
    }
    GetCanvasBoundingRect() {
        this.BRectLeft = document
            .getElementById(this.canvasId)
            .getBoundingClientRect().left;
        this.BRectTop = document
            .getElementById(this.canvasId)
            .getBoundingClientRect().top;
    }
    draw(currX, currY) {
        //this.windowResize();
        var scaleX, scaleY, rect;
        rect = document.getElementById(this.canvasId).getBoundingClientRect();

        scaleX = this.canvas.width / rect.width;
        scaleY = this.canvas.height / rect.height;
        this.ctx.beginPath();

        this.ctx.moveTo((this.prevX - this.BRectLeft) * scaleX, (this.prevY - this.BRectTop) * scaleY);
        this.ctx.lineTo((currX - this.BRectLeft) * scaleX, (currY - this.BRectTop) * scaleY);
        //ctx.moveTo(prevX - BRectLeft, prevY - BRectTop);
        //ctx.lineTo(currX - BRectLeft, currY - BRectTop);
        this.ctx.strokeStyle = this.cuurentStrokStyle;;
        this.ctx.lineWidth = 2;
        this.ctx.stroke();
        this.ctx.closePath();
        this.lastDraw.x = currX;
        this.lastDraw.y = currY;
        this.points = [];
        this.points.push({ x: this.prevX, y: this.prevY });
        this.points.push({ x: currX, y: currY });

        this.pathsarray.push(this.points);
        this.prevX = currX;
        this.prevY = currY;
        return '';
    }

    drawPath() {
        this.clearCanvas();
        var dpoint;
        this.ctx.beginPath();
        this.ctx.strokeStyle = this.cuurentStrokStyle;
        this.pathsarray.forEach((path) => {
            dpoint = this.GetDrawingPoint(path[0]);
            this.ctx.moveTo(dpoint.x, dpoint.y);
            for (let i = 1; i < path.length; i++) {
                dpoint = this.GetDrawingPoint(path[i]);
                this.ctx.lineTo(dpoint.x, dpoint.y);
                this.prevX = path[i].x;
                this.prevY = path[i].y;
            }
            this.ctx.stroke();
        });
        this.ctx.closePath();

    }
    GetDrawingPoint(point) {
        var scaleX, scaleY, rect;
        rect = document.getElementById(this.canvasId).getBoundingClientRect();

        scaleX = this.canvas.width / rect.width;
        scaleY = this.canvas.height / rect.height;
        return {
            x: (point.x - this.BRectLeft) * scaleX, y: (point.y - this.BRectTop) * scaleY
        }
    }

    clearCanvas() {
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.scaleToFit(this.img);
        this.SetNewLine();
    }
    clearDraw() {
        this.points = [];
        this.pathsarray = [];
        //newline();
        this.drawPath();
    }
    SetNewLine() {
        this.newLine = true;
        this.bDrawOnMove = false;
        this.lastDraw = { x: -1, y: -1 };
    }
    setImage(imagId) {
        this.img = document.getElementById(imagId);
        if (this.img != null) {
            this.imageId = imagId;
            this.scaleToFit(this.img);
        }


    }
    scaleToFit(img) {
        if (img === undefined)
            return;
        // get the scale
        if (this.scaleMode == 0) {
            this.ctx.drawImage(this.img, 0, 0);
        } else if (this.scaleMode == 1) {
            var scaleWidth = this.canvas.width / this.img.width;
            this.ctx.drawImage(
                this.img,
                0,
                0,
                this.img.width * scaleWidth,
                this.img.height * scaleWidth
            );
        } else if ((this.scaleMode = 2)) {
            var scaleWidth = this.canvas.width / this.img.width;
            var scaleHeight = this.canvas.height / this.img.height;
            this.ctx.drawImage(
                this.img,
                0,
                0,
                this.img.width * scaleWidth,
                this.img.height * scaleHeight
            );
        } else {
            var scale = Math.min(
                this.canvas.width / this.img.width,
                this.canvas.height / this.img.height
            );
            // get the top left position of the image
            var x = this.canvas.width / 2 - (this.img.width / 2) * scale;
            var y = this.canvas.height / 2 - (this.img.height / 2) * scale;
            this.ctx.drawImage(this.img, x, y, this.img.width * scale, this.img.height * scale);
        }
    }
}
