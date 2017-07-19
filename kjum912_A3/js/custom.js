
$('#myModal').on('shown.bs.modal', function () {
  $('#myModal').modal('show');

  document.addEventListener("DOMContentLoaded", function(){
      // Get the submit button from the html by its id
      var myButton = document.getElementById("myBtn");

      // When the submit button is clicked, execute myFunction
      myButton.addEventListener("click", myFunction);

      function myFunction(){
          // This function is our event handler. An alert shows when the submit button is clicked.
          alert("My button was clicked");
      }
  });
