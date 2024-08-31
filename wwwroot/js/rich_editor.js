const richEditor = document.getElementById("rich-editor");
let quill = null;
if (richEditor != null) {
    quill = new Quill('#rich-editor', {
        theme: 'snow'
    });
}
//const form = document.getElementById("form");
//form.onsubmit = function () {
//    if (details != null)
//        details.value = quill.getSemanticHTML();
//}

window.fillOutRichEditor = function (fieldSource) {
    const field = document.getElementById(fieldSource);
    if (field != null && quill != null) {
        quill.root.innerHTML = details.value;
    }
}

window.saveRichEditorText = function (fieldName) {
    const form = document.getElementById("form");
    const field = document.getElementById(fieldName);
    if (form != null && field != null && quill != null) {
        form.onsubmit = function () {
            field.value = quill.getSemanticHTML();
        }
    }
}    