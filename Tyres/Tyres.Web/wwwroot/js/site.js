function grayBackground(item) {
    item.style.background = "grey";
}

function whiteBackground(item) {
    item.style.background = "white";
}

function TyresCalculator() {

    let oldWidth = document.getElementById("OldWidth").value;
    let oldRatio = document.getElementById("OldRatio").value;
    let oldRimDiameter = document.getElementById("OldRimDiameter").value;

    let newWidth = document.getElementById("NewWidth").value;
    let newRatio = document.getElementById("NewRatio").value;
    let newRimDiameter = document.getElementById("NewRimDiameter").value;

    function calculateFullDiameter(width, ratio, rim) {
        let centimetersInOneInch = 2.54;
        let millimetersInOneCemtimeter = 10;

        let rimInCentimetres = rim * centimetersInOneInch * millimetersInOneCemtimeter;
        let profileHeight = width * ratio / 100;

        let fullDiameter = rimInCentimetres + profileHeight * 2;

        return fullDiameter;
    }

    let oldFullDiameter = calculateFullDiameter(oldWidth, oldRatio, oldRimDiameter);
    let newFullDiameter = calculateFullDiameter(newWidth, newRatio, newRimDiameter);

    let diametersDifference = newFullDiameter - oldFullDiameter;

    let deviation = 1 - oldFullDiameter / newFullDiameter;
    let maximumDeviation = 0.03;

    let compatibilityConclusion;
    let backgroundColor;

    if (Math.abs(deviation) > maximumDeviation) {
        backgroundColor = "#FFB9B9";
        compatibilityConclusion = "incompatible";
    }
    else {
        backgroundColor = "#BADCAB";
        compatibilityConclusion = "compatible";
    }

    let kmPerHourSpeed = 100;
    let speedometerReadings = kmPerHourSpeed * (1 - deviation);

    document
        .getElementById("OldFullDiameter")
        .innerHTML = `${Math.round(oldFullDiameter)} mm`;

    document
        .getElementById("NewFullDiameter")
        .innerHTML = `${Math.round(newFullDiameter)} mm`;

    document
        .getElementById("Difference")
        .innerHTML = `${Math.round(diametersDifference)} mm`;

    document
        .getElementById("SpeedometerSpeed")
        .innerHTML = `${Math.round(speedometerReadings)} km`;

    document
        .getElementById("Compatibility")
        .innerHTML = `<span style="background-color:${backgroundColor}">${compatibilityConclusion}</span>`;
}


