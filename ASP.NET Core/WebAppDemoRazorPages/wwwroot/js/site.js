// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function rowClick(id) {
    let el = document.getElementById(id);
    el.classList.toggle("selectedRow")
}

function getSelectedRow(){
   return document.getElementsByClassName("selectedRow");
}

function deleteItems(tbId) {
    let items = getSelectedRow();
    let tb = document.getElementById(tbId);
    for (let i = 0; i < items.length; i++) {
        let id = items[i].id.substring(2);
        $.ajax({
            url: '/api/organizations/' + id,
            type: 'DELETE',
            success: () => {
                let row = document.getElementById("id" + id);
                tb.removeChild(row);
                console.log("Delete item with id=" + id);
            },
            error: () => {
                console.log("Error delete item with id=" + id);
            }

        })
    }
    //RefreshOrganizationTable(tbId);
}
var PageSize = 20;
var Skip = 0;
var Count = 0;
function RefreshOrganizationTable(tbId) {
    let xhr = new XMLHttpRequest();
    xhr.open('GET', '/api/organizations');
    xhr.responseType = 'json';
    xhr.onload = function () {
        let status = xhr.status;
        if (status == 200) {
            let data = xhr.response;
            let tb = document.getElementById(tbId);
            tb.innerHTML = null;
            for (let i = 0; i < data.length; i++) {
                let tr = document.createElement("tr");
                let id = "id" + data[i].id;
                tr.id = id;
                tr.onclick = () => rowClick(id);
                tb.appendChild(tr);
                let tdName = document.createElement("td");
                tr.appendChild(tdName);
                tdName.innerHTML = data[i].name;
                let tdFullName = document.createElement("td");
                tr.appendChild(tdFullName);
                tdFullName.innerHTML = data[i].fullName;
                let tdCreated = document.createElement("td");
                tr.appendChild(tdCreated);
                tdCreated.innerHTML = data[i].created;

            }
        }
    }
    xhr.send();
}