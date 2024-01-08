// start get user ıdentity kind
let dropdownList = document.getElementById('userIdentitySelection');
dropdownList.onchange = (_) =>{
    let selecetedIndex = dropdownList.selectedIndex;
    document.getElementById("userIdentityInput").disabled=selecetedIndex === 0;
    // console.log("Selected index is: " + selecetedIndex);
    // let selectedOption = dropdownList.options[selecetedIndex];
    // console.log("Selected option is: " + selectedOption.outerHTML);
    // console.log("Selected value is: " + selectedOption.value);
    // console.log("Selected text is: " + selectedOption.text);
}
// end get user ıdentity kind