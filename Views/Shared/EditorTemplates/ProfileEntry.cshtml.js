const quill = new Quill('#rich-editor', {
    theme: 'snow'
});
const form = document.getElementById("form");
const details = document.getElementById("details-area");

const currentCheckBox = document.getElementById("current-check");
const endDateInput = document.getElementById("end-date-input");

toggleEndDateInput(currentCheckBox.checked);

function toggleEndDateInput() {
    endDateInput.disabled = currentCheckBox.checked;
}


//Details
if (details != null) {
    quill.root.innerHTML = details.value;
}

form.onsubmit = function () {
    if (details != null)
        details.value = quill.getSemanticHTML();
}    