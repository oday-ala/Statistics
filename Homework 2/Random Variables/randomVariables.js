"use strict";
document
  .getElementById("csvFile")
  .addEventListener("change", handleFileSelect, false);
function handleFileSelect(event) {
  const file = event.target.files[0];
  const reader = new FileReader();

  reader.onload = function (e) {
    const content = e.target.result;
    const data = parseCSV(content);

    const totalDataPoints = {
      qualitativePoints: data.qualitativeData.length,
      discretePoints: data.discreteData.length,
      continuousPoints: data.continuousData.length,
    };

    const qualitativeFrequency = computeFrequencies(
      data.qualitativeData,
      totalDataPoints.qualitativePoints
    );
    const discreteFrequency = computeFrequencies(
      data.discreteData,
      totalDataPoints.discretePoints
    );
    const classInterval = 0.01;

    const continuousFrequency = computeFrequencies(
      data.continuousData,
      totalDataPoints.continuousPoints,
      true,
      classInterval
    );
    //   joint Frequency where k = 2 for two variables
    const jointFrequency = computeJointFrequency([
      data.qualitativeData,
      data.discreteData,
    ]);
    displayTable("Qualitative Variable", "Dream Works", qualitativeFrequency);
    displayTable("Discrete Variable", "Hard Worker", discreteFrequency);
    displayTable("Continuous Variable", "Height", continuousFrequency);
    displayTable("Joint Frequency", "jointFrequency", jointFrequency);
  };

  reader.readAsText(file);
}

function computeFrequencies(
  data,
  totalDataPoints,
  isContinuous = false,
  classInterval = 1
) {
  // Compute absolute frequency
  let absoluteFrequency = {};
  if (isContinuous) {
    // For continuous variables, group data into class intervals
    absoluteFrequency = data.reduce((acc, item) => {
      const interval = (
        Math.floor(item / classInterval) * classInterval
      ).toFixed(2);
      acc[interval] = (acc[interval] || 0) + 1;
      return acc;
    }, {});
  } else {
    data.forEach((item) => {
      if (!absoluteFrequency[item]) {
        absoluteFrequency[item] = 1;
      } else {
        absoluteFrequency[item]++;
      }
    });
  }
  // Compute relative and percentage frequencies
  const relativeFrequency = {};
  const percentageFrequency = {};
  for (const category in absoluteFrequency) {
    relativeFrequency[category] = absoluteFrequency[category] / totalDataPoints;
    percentageFrequency[category] =
      (relativeFrequency[category] * 100).toFixed(2) + "%";
  }

  return {
    absolute: absoluteFrequency,
    relative: relativeFrequency,
    percentage: percentageFrequency,
  };
}

function parseCSV(content) {
  const lines = content.split("\n");
  const qualitativeData = [];
  const discreteData = [];
  const continuousData = [];

  for (let i = 1; i < lines.length; i++) {
    const columns = lines[i]
      .split(/,(?=(?:[^"]*"[^"]*")*[^"]*$)/)
      .map((item) => item.trim());
    const qualitative = columns[25];
    const discrete = columns[4];
    const continuous = columns[18];

    qualitativeData.push(qualitative);
    discreteData.push(parseInt(discrete, 10));
    continuousData.push(parseFloat(continuous));
  }

  return { qualitativeData, discreteData, continuousData };
}

function computeJointFrequency(dataArrays) {
  // Initialize a joint frequency object
  const jointFrequency = {};

  // Iterate through the data arrays
  for (let i = 0; i < dataArrays[0].length; i++) {
    // Create a key by joining values from all arrays
    const key = dataArrays.map((dataArray) => dataArray[i]).join(" & ");

    // Update the joint frequency
    if (!jointFrequency[key]) {
      jointFrequency[key] = 1;
    } else {
      jointFrequency[key]++;
    }
  }

  // Compute total data points
  const totalDataPoints = Object.values(jointFrequency).reduce(
    (acc, freq) => acc + freq,
    0
  );

  // Compute relative frequency and percentage frequency
  const relativeFrequency = {};
  const percentageFrequency = {};
  for (const key in jointFrequency) {
    relativeFrequency[key] = jointFrequency[key] / totalDataPoints;
    percentageFrequency[key] = (relativeFrequency[key] * 100).toFixed(2) + "%";
  }

  return {
    absolute: jointFrequency,
    relative: relativeFrequency,
    percentage: percentageFrequency,
  };
}

function displayTable(title, subTitle, frequencies) {
  const resultDiv = document.getElementById("results");
  const table = document.createElement("table");

  const headerRow = table.insertRow();
  const headerCell = headerRow.insertCell();
  headerCell.textContent = title;
  headerCell.colSpan = 4;

  const headerRow2 = table.insertRow();
  const categoryCell = headerRow2.insertCell();
  categoryCell.textContent = subTitle;

  const absoluteCell = headerRow2.insertCell();
  absoluteCell.textContent = "Absolute";

  const relativeCell = headerRow2.insertCell();
  relativeCell.textContent = "Relative";

  const percentageCell = headerRow2.insertCell();
  percentageCell.textContent = "Percentage";

  for (const category in frequencies.absolute) {
    const row = table.insertRow();
    const categoryCell = row.insertCell();
    categoryCell.textContent = category;
    console.log(frequencies.absolute);
    const absoluteCell = row.insertCell();
    absoluteCell.textContent = frequencies.absolute[category];

    const relativeCell = row.insertCell();
    relativeCell.textContent = frequencies.relative[category];

    const percentageCell = row.insertCell();
    percentageCell.textContent = frequencies.percentage[category];
  }

  // Append the table to the results div
  resultDiv.appendChild(table);
}
