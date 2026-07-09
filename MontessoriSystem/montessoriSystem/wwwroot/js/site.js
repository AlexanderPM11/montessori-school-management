// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


let code = document.getElementById("codeTxt");
let codeHidden = document.getElementById("codeHidden");
let assetTypeHidden = document.getElementById("assetTypeHidden");
let minHidden = document.getElementById("minHidden");
let maxHidden = document.getElementById("maxHidden");
let bathroomHidden = document.getElementById("bathroomHidden");
let bedroomHidden = document.getElementById("bedroomHidden");

function GetCodeValue() {
    codeHidden.value = code.value;
}

function GetTypeIdValue(id) {
    assetTypeHidden.value = id;
}

/// function to open and close dashboard
//const btn_Close = document.querySelector(".cont_Btn_bar");
//btn_Close.addEventListener("click", () => {
//    const dashboard = document.querySelector(".dashboard");
//    const containerrenderbody = document.querySelector(".container-render-body");

//    containerrenderbody.style.marginLeft = "30px"
//    dashboard.style.display = "none";
    
//})
//const btn_Open = document.querySelector(".Cont-btn_Opne");
//btn_Open.addEventListener("click", () => {
//    const dashboard = document.querySelector(".dashboard");
//    const containerrenderbody = document.querySelector(".container-render-body");
//    const containernavbartitleDesc = document.querySelector(".container-navbar-nameApp");
//    dashboard.style.display = "Block";    
//    containerrenderbody.style.marginLeft = "320px"
//    containernavbartitleDesc.style.marginLeft = "285px";    
    
//})


function getSelectOptionMenu() {
    var valorLocalStorage = localStorage.getItem("optionSelect");
    if (valorLocalStorage != null && valorLocalStorage != '') {
        var p = document.querySelector(".p-page-current");
        p.textContent = valorLocalStorage;
        console.log(p.textContent)
    }
    
}
function deleteSelectOptionMenu() {
  localStorage.removeItem("optionSelect");
}

var elementosA = document.querySelectorAll('.dashboard-ul-list .nav-item.dashboard-cont-link a');
elementosA.forEach(function (elemento) {
    elemento.addEventListener("click", function (event) {
        var text = "General / " + elemento.textContent
        localStorage.setItem("optionSelect", text);
        var p = document.querySelector(".p-page-current");
        p.textContent = text;
    });
});

function setPageCurretn(page) {
   
    localStorage.setItem("optionSelect", page);
}



//This code is for the FormImmovable View.
const multiSelectInitializer = () => {

    const multiSelect = new IconicMultiSelect({
        customCss: true,
        select: "#improvement_list"
    });

    multiSelect.subscribe(function (evt) {

        switch (evt.action) {
            case 'ADD_OPTION':
                for (i = 0; i < multiSelect._selectContainer.options.length; i++) {
                    if (multiSelect._selectContainer.options[i].value == evt.value) {
                        multiSelect._selectContainer.options[i].selected = true
                        multiSelect._selectContainer.options[i].setAttribute("selected", "")
                    }
                }
                break;
            case 'REMOVE_OPTION':
                for (i = 0; i < multiSelect._selectContainer.options.length; i++) {
                    if (multiSelect._selectContainer.options[i].value == evt.value) {
                        multiSelect._selectContainer.options[i].selected = false
                        multiSelect._selectContainer.options[i].removeAttribute("selected", "")
                    }
                }
                break;
        }

    });

    multiSelect.init();

}

