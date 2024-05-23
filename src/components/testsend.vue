<template>
    <!-- Your form template -->
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    data() {
      return {
        name: '',
        color: '',
        shape: ''
      };
    },
    methods: {
      sendData() {
        const data = {
          name: this.name,
          color: this.color,
          shape: this.shape
        };
  
        axios.post('http://localhost:5298/api/data', data)
          .then(response => {
            console.log(response.data); // Log the response from the server
            // Optionally, handle the response here (e.g., show a success message to the user)
          })
          .catch(error => {
            console.error('Error sending data:', error);
            // Optionally, handle the error here (e.g., show an error message to the user)
          });
      }
    }
  };
  </script>
  