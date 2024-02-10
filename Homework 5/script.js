<!DOCTYPE html>

<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <title>Security Score Animation</title>
</head>

<body>
    <form id="Form">
        <label>Number of systems</label>
        <input type="number" id="M" value="50" min="1" />
        <br /> <br />
        <label>Attacks rate</label>
        <input type="number" id="rate" value="1" min="1" />
        <br /> <br />
        <label>Intervals</label>
        <input type="number" id="intervals" value="10" min="1" />
        <br /> <br />
    </form>
    <button onclick="check()">Start Attacks!</button>
    <canvas id="scoreCanvas" width="1920" height="1080" style="border: 2px solid black;"></canvas>
    <script>
        "use strict";
        class Interval {

            constructor(upper, lower, count) {
                this.upper = upper;
                this.lower = lower;
                this.count = count;

            }

        }

        class Rectangle {
            constructor(x, y, width, height) {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;

            }


            left() {
                return this.x
            }


            top() {
                return this.y
            }


            right() {
                return this.x + this.width
            }


            bottom() {
                return this.y + this.height
            }


            aspectRatio() {
                return this.width / this.height || 1
            }

            drawRectangle(ctx, lineColor, lineWidth, lineDash) {
                ctx.save();
                ctx.beginPath();
                ctx.rect(this.x, this.y, this.width, this.height);
                ctx.strokeStyle = lineColor;
                ctx.lineWidth = lineWidth;
                ctx.setLineDash(lineDash);
                ctx.stroke();
                ctx.restore()
            }

        }


        const viewRect = new Rectangle(20, 30, 1600, 1000);

        class Prof2dUtilities {
            static transformX(x, min_x, rangeL_x, left, width) {
                return left + width * (x - min_x) / rangeL_x;
            }
            static transformY(y, min_y, rangeL_y, top, height) {
                return top + height - (height * (y - min_y) / rangeL_y);

            }

        }


        class Histogram {
            static verticalHistoFromIntervals(ctx, intervals, y_min, y_range, viewRect, strokeStyle, lineWidth, fillStyle) {
                let maxcount = 0;

                for (const interval of intervals) {
                    maxcount = Math.max(maxcount, interval.count);
                }

                for (const interval of intervals) {
                    let x_rect = viewRect.x;
                    let width_rect = viewRect.width * interval.count / maxcount;
                    let y_rect_top = Prof2dUtilities.transformY(interval.upper, y_min, y_range, viewRect.y, viewRect.height);
                    let y_rect_bottom = Prof2dUtilities.transformY(interval.lower, y_min, y_range, viewRect.y, viewRect.height);
                    let height_rect = y_rect_bottom - y_rect_top;
                    let rectInterval = new Rectangle(x_rect, y_rect_top, width_rect, height_rect);

                    ctx.rect(rectInterval.x, rectInterval.y, rectInterval.width, rectInterval.height);

                    const gradient = ctx.createLinearGradient(rectInterval.x, rectInterval.y, rectInterval.x, rectInterval.y + rectInterval.height);
                    gradient.addColorStop(0, 'blue');
                    gradient.addColorStop(0.25, 'blue');
                    gradient.addColorStop(0.5, 'blue');
                    gradient.addColorStop(0.75, 'blue');
                    gradient.addColorStop(1, 'blue');
                    ctx.fillStyle = gradient;
                    ctx.fillRect(rectInterval.x, rectInterval.y, rectInterval.width, rectInterval.height);

                }

            }

        }

        //setInterval(update, 1000 / 60);
        const canvas = document.getElementById("scoreCanvas");
        const ctx = canvas.getContext("2d");
        addHandlersForResize(canvas);


        const start = document.getElementById("start");

        // start.onclick = check();

        function check() {
            let currentAttack = 1;
            let numSystems = parseInt(document.getElementById('M').value);
            const T = 1;
            var rate = parseFloat(document.getElementById('rate').value);
            var intervals = parseInt(document.getElementById('intervals').value);
            var penetrationProbability = rate * (T / intervals);
            let numAttacks = intervals;

            if (isNaN(numSystems) | numSystems < 1) {
                alert("INVALID NUMBER OF SYSTEMS");
                return false;
            }
            if (isNaN(rate) | rate < 1 | rate > intervals) {
                alert("INVALID RATE OF ATTACKS");
                return false;
            }
            if (isNaN(intervals) | intervals < 1) {
                alert("INVALID NUMBER OF INTERVALS");
                return false;
            }

            //const p = rate * (T / intervals);
            // let cumulatedFN = calculateNormalizedFrequency(cumulatedF);
            let securityTrajectories = calculateSecScore(numSystems, numAttacks, penetrationProbability);

            let allTrajectories = securityTrajectories.flat();
            let y_max = Math.max(...allTrajectories);
            let y_min = 0;
            //let y_range = Math.max(Math.abs(y_max), Math.abs(y_min)) * 2;
            let y_range = y_max - y_min;
            //y_min = Math.abs(y_max) > Math.abs(y_min) ? -y_max : y_min;

            function startSimulation() {



                if (currentAttack <= numAttacks) {
                    ctx.clearRect(0, 0, canvas.width, canvas.height);

                    viewRect.drawRectangle(ctx, "black", 2, [1, 1]);
                    drawScores(securityTrajectories, currentAttack, Math.abs(y_min), y_range, viewRect, numSystems);
                    drawLabels(viewRect, y_range);


                    if (currentAttack >= Math.floor(numAttacks / 2)) {

                        printHistogram(Math.floor(numAttacks / 2), securityTrajectories);

                    }


                    if (currentAttack >= numAttacks) {
                        printHistogram(numAttacks, securityTrajectories);
                    }

                    currentAttack++;

                    if (currentAttack <= numAttacks) {
                        setTimeout(startSimulation, frameDuration);
                    }

                } else {
                    currentAttack = 1;
                    startSimulation();
                }
            }

            let frameDuration = 0;
            startSimulation();

            function calculateSecScore(numSystems, numAttacks, penetrationProbability) {
                var securityTrajectories = [];

                for (let system = 0; system < numSystems; system++) {
                    var scores = [];
                    let score = 0;

                    for (let attack = 0; attack < numAttacks; attack++) {
                        const outcome = Math.random() <= penetrationProbability ? 0 : 1;
                        score += outcome;
                        scores.push(score);
                    }
                    securityTrajectories.push(scores);

                }
                console.log("securityTrajectory are:", securityTrajectories);
                return securityTrajectories;

            }


            function drawScores(data, frame, y_min, y_range, viewRect, numSystems) {
                for (let system = 0; system < numSystems; system++) {
                    const currentTrajectory = data[system];
                    const x_start = viewRect.left();
                    const x_step = viewRect.width / numAttacks;
                    const y_start = viewRect.bottom() - viewRect.height;
                    let x = x_start;
                    let y = Prof2dUtilities.transformY(currentTrajectory[0], y_min, y_range, y_start, viewRect.height);
                    for (let i = 0; i <= frame; i++) {
                        const outcome = currentTrajectory[i];
                        ctx.strokeStyle = "#" + ((1 << 24) * Math.random() | 0).toString(16);
                        ctx.beginPath();
                        ctx.moveTo(x, y);
                        x += x_step;
                        y = Prof2dUtilities.transformY(outcome, y_min, y_range, y_start, viewRect.height);
                        ctx.lineTo(x, y);
                        ctx.stroke();
                    }

                }

            }


            function drawLabels(rect, yrange) {

                const ctx = canvas.getContext("2d");
                const xAxisLabelY = rect.bottom() + 10;
                const yAxisLabelX = rect.left() - 10;
                const labelPadding = 10;
                const yrangeHalf = yrange / 2;
                const yStep = Math.floor(yrange / 4);
                const xStep = Math.floor(numAttacks / 5);

                for (let i = 0; i <= numAttacks; i += xStep) {
                    const x = rect.left() + i * (rect.width / numAttacks);
                    const label = i.toString();
                    ctx.fillStyle = "black";
                    ctx.fillText(label, x - labelPadding, xAxisLabelY);

                }

                var y;
                for (let i = 0; i <= yrange; i += yStep) {
                    if (i == 0) { y = rect.bottom(); }
                    else { y = rect.bottom() - (i + yrangeHalf / 8) * (rect.height / yrange); }
                    const label = i.toString();
                    ctx.fillStyle = "black";
                    ctx.fillText(label, yAxisLabelX, y);

                }


                ctx.stroke();

            }



            function calculateIntervals(data, systemIndex, numIntervals) {
                var systemData = data.map(subArray => subArray[systemIndex - 1]);
                var minValue = Math.min(...systemData);
                var maxValue = Math.max(...systemData);
                var intervalSize = 1;

                var intervals = [];

                for (let i = 0; i < numIntervals; i++) {
                    var lower = minValue + i * intervalSize;
                    var upper = lower + intervalSize;
                    var count = systemData.filter((value) => value >= lower && value < upper).length;

                    var interval = new Interval(upper, lower, count);
                    intervals.push(interval);

                }


                intervals.sort((a, b) => a.upper - b.upper);
                return intervals;

            }


            function printHistogram(numAttack, securityTrajectories) {

                var intervals = calculateIntervals(securityTrajectories, numAttack, numSystems + 2);
                let histoHeight = viewRect.height;
                let y_offset = (viewRect.height - histoHeight) * 0.5;
                var histoRect1 = new Rectangle(Prof2dUtilities.transformX(numAttack, 0, numAttacks, viewRect.x, viewRect.width), viewRect.y + y_offset, 150, histoHeight);
                Histogram.verticalHistoFromIntervals(ctx, intervals, y_min, y_range, histoRect1, "yellow", 3, "yellow");

            }



        }



        // Implements the drag of the graphs
        $(document).ready(function () {
            $("#scoreCanvas").draggable();
        });

        // Implements the resize of the graphs
        let isResizing = false;
        let resizeOffsetX, resizeOffsetY;

        function addHandlersForResize(element) {

            element.addEventListener("mousedown", (event) => {
                if (event.button === 2) {
                    const rect = element.getBoundingClientRect();

                    resizeOffsetX = event.clientX - rect.right;
                    resizeOffsetY = event.clientY - rect.bottom;
                    isResizing = true;
                }
            });

            element.addEventListener("mousemove", (event) => {
                if (isResizing) {
                    element.style.width = event.clientX - element.getBoundingClientRect().left - resizeOffsetX + "px";
                    element.style.height = event.clientY - element.getBoundingClientRect().top - resizeOffsetY + "px";
                }
            });

            element.addEventListener("mouseup", () => {
                isResizing = false;
            });


            element.addEventListener("resize", () => {
                element.style.width = container.clientWidth + "px";
                element.style.height = container.clientHeight + "px";
            });
        }




    </script>

</body>

</html>