
let pageNo = 1;
let limit = 4;

let previousButton, nextButton, pageNumberSpan, tableBody;
document.addEventListener("DOMContentLoaded", function () {
  pageNumberSpan = document.querySelector("#pageNumberSpan");
  tableBody = document.getElementById("tableBody");
  nextButton = document.querySelector("#nextBtn");
  previousButton = document.querySelector("#previousBtn");

  pageNumberSpan.innerHTML = pageNo;

  nextButton.addEventListener("click", nextButtonHandler);
  previousButton.addEventListener("click", previousButtonHandler);
  getTotalEmployees();
  disableButtonHandler();

  // Initial fetch and binding
  fetchData(pageNo, limit);
});

var totalData = 0;

function getTotalEmployees() {
  $.get("https://localhost:44365/api/employee/count", function (data) {
    totalData = data.count;
  });
}

function fetchData(pageNumber, lt) {
  $.get(
    `https://localhost:44365/api/employee/get-all-employees?pageNumber=${pageNumber}&limit=${lt}`,
    function (data) {
      console.log("data", data);
      bindData(data);
    }
  ).fail(function (error) {
    console.error("Error fetching data:", error);
  });
}

function nextButtonHandler() {
  pageNo++;
  disableButtonHandler();
  pageNumberSpan.innerHTML = pageNo;

  fetchData(pageNo, limit);
}

function previousButtonHandler() {

    pageNo--;
    disableButtonHandler();
    pageNumberSpan.innerHTML = pageNo;
    fetchData(pageNo, limit);

}

function bindData(responseData) {
  let html = "";
  responseData.forEach((data) => {
    const { Id, Name, Department, Email } = data;

    html += `
      <tr>
        <td>${Id}</td>
        <td>${Name}</td>
        <td>${Department}</td>
        <td>${Email}</td>
      </tr>
    `;
  });

  // Set the innerHTML of tableBody to the generated HTML
  tableBody.innerHTML = html;
}

function disableButtonHandler() {
  if (pageNo === 1) {
    previousButton.disabled = true;
  } else {
    previousButton.disabled = false;
  }

  if (pageNo === Math.ceil(totalData / limit)) {
    console.log("curent", totalData / limit);
    nextButton.disabled = true;
  } else {
    nextButton.disabled = false;
  }
}
