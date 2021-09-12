


//Всплывающее модальное окно, время появления указано в контролере. 
//Встроен INPUT формы для сабмита данных кнопка"сохранить страну".



function launchMyForm() {
    var modalNew = document.createElement("div");
    modalNew.setAttribute("class", "modal fade");
    modalNew.setAttribute("style", "opacity: 1");
    modalNew.setAttribute("id", "exampleModal");
    modalNew.setAttribute("tabindex", "-1");
    modalNew.setAttribute("aria-labelledby", "exampleModalLabel");
    modalNew.setAttribute("aria-hidden", "true");
    document.getElementById("TestForm").appendChild(modalNew);

    var modalDialog = document.createElement("div");
    modalDialog.setAttribute("class", "modal-dialog");
    modalDialog.setAttribute("id", "modal-dialog");
    document.getElementById("exampleModal").appendChild(modalDialog);

    var modalContent = document.createElement("div");
    modalContent.setAttribute("class", "modal-content");
    modalContent.setAttribute("id", "modalcontent");
    document.getElementById("modal-dialog").appendChild(modalContent);

    var modalHeader = document.createElement("div");
    modalHeader.setAttribute("class", "modal-header");
    modalHeader.setAttribute("id", "modal-header");
    document.getElementById("modalcontent").appendChild(modalHeader);

    var title = document.createElement("h5");
    title.innerHTML = "Сохранить";
    title.setAttribute("class", "modal-title");
    title.setAttribute("id", "exampleModalLabel");
    document.getElementById("modal-header").appendChild(title);


    var buttonModal = document.createElement("button");
    buttonModal.setAttribute("type", "button");
    buttonModal.setAttribute("class", "btn-close");
    buttonModal.setAttribute("onclick", "buttonCancel()");
    buttonModal.setAttribute("data-bs-dismiss", "modal");
    buttonModal.setAttribute("aria-label", "Close");
    document.getElementById("modal-header").appendChild(buttonModal);

    var modalBody = document.createElement("div");
    modalBody.innerHTML = "Можете сохранить страну в базе данных";
    modalBody.setAttribute("class", "modal-body");
    modalBody.setAttribute("id", "modalBody");
    document.getElementById("modalcontent").appendChild(modalBody);

    var modalFooter = document.createElement("div");
    modalFooter.setAttribute("class", "modal-footer");
    modalFooter.setAttribute("id", "modalFooter");
    document.getElementById("modalcontent").appendChild(modalFooter);

    var buttonCancel = document.createElement("button");
    buttonCancel.setAttribute("type", "button");
    buttonCancel.setAttribute("onclick", "buttonCancel()");
    buttonCancel.innerHTML = "Отмена";
    buttonCancel.setAttribute("class", "btn btn-secondary");
    buttonCancel.setAttribute("data-bs-dismiss", "modal");
    document.getElementById("modalFooter").appendChild(buttonCancel);

     // <INPUT id="MyInput" type="submit" value="Сохранить страну" />
    var myInput = document.createElement("INPUT");
    myInput.setAttribute("id", "MyInput");
    myInput.setAttribute("type", "submit");
    myInput.setAttribute("value", "Сохранить страну");
    myInput.setAttribute("class", "btn btn-primary");
    document.getElementById("modalFooter").appendChild(myInput);
}

//удаление модалки при нажатии close или отмены.
function buttonCancel() {
    document.getElementById("exampleModal").remove();
}

