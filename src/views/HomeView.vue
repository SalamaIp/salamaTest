<template>
  <div class="container">
    <div class="form">
      <h2>Add Data</h2>
      <div class="form-group">
        <label for="name">Name:</label>
        <input type="text" id="name" v-model="name" required />
      </div>

      <div class="form-group">
        <label for="color">Colors:</label>
        <select id="color" v-model="color" required>
          <option value="" disabled selected>Select a color</option>
          <option value=1>Red</option>
          <option value=2>blue </option>
          <option value=3>Yellow</option>
          <option value=4>black</option>
          <option value=5>white</option>
          <option value=6>Green</option>
        </select>
      </div>

      <div class="form-group">
        <label for="shape">Shapes:</label>
        <select id="shape" v-model="shape" required>
          <option value="" disabled selected>Select a shape</option>
          <option value=1>circle</option>
          <option value=2>Triangle</option>
          <option value=3>Rectangle</option>
          <option value=4>Square</option>
          <option value=5>Cone</option>
          <option value=6>Sphere</option>
        </select>
      </div>
      <button v-if="showOkButton" class="btn btn-ok" @click="updateData(this.refIndex)">OK</button>
      <button :disabled="showOkButton" class="btn" @click="sendData">Add to List</button>
    </div>

    <div class="data-list">
      <h2>Added Data List</h2>
      <ul class="list-group">
        <li v-for="(item, index) in dataList" :key="index" class="list-item">
          <strong>{{ index + 1 }}:</strong> Name: {{ item.name }}, Colors: {{ item.color }}, Shapes: {{ item.shape }}
          <button class="btn btn-edit" @click="editItem(index)">Edit</button>
          <button class="btn btn-delete" @click="deleteItem(this.dataList[index].id)">Delete</button>

        </li>
      </ul>
      <button class="btn btn-info" @click="takeQuiz">Take a Quiz</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      id: "",
      name: "",
      color: "",
      shape: "",
      dataList: [],
      refIndex: 0,
      showOkButton: false
    };
  },
  created() {
    this.fetchData();
  },
  methods: {
    async fetchData() {
      try {
        let response = await axios.get("https://localhost:7105/api/Quiz/Test");
        if (Array.isArray(response.data)) {
          response.data.forEach(obj => {

            const color = this.getColorById(obj.colorId);
            const shape = this.getShapeById(obj.shapeId);

            this.dataList.push({
              id: obj.id,
              name: obj.name,
              color: color,
              shape: shape
            });
          });
        } else {
          console.warn("Received data is not in the expected format.");
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    },

    async sendData() {
      if (this.name && this.color && this.shape) {
        try {
          let result = await axios.post("https://localhost:7105/api/Quiz/PostAPI", {
            Name: this.name,
            ColorId: this.color,
            ShapeId: this.shape
          });
          const color = this.getColorById(result.data.colorId);
          const shape = this.getShapeById(result.data.shapeId);

          this.dataList.push({
            id: result.data.id,
            name: result.data.name,
            color: color,
            shape: shape
          });
          this.name = "";
          this.color = "";
          this.shape = "";
        } catch (error) {
          console.error("Error sending data:", error);
        }
      } else {
        alert("Please fill in all fields.");
      }
    },

    async deleteItem(index) {
      try {
        let result = await axios.post(`https://localhost:7105/api/Quiz/delete/${index}`);
        console.warn(result);
        window.location.reload();
      } catch (error) {
        console.error("Error in delete data:", error);
      }
    },

    async updateData(index) {
      if (this.name && this.color && this.shape) {
        try {
          let result = await axios.post(`https://localhost:7105/api/Quiz/update/${this.dataList[index].id}`,
            {
              Name: this.name,
              ColorId: this.color,
              ShapeId: this.shape
            }
          );
          const color = this.getColorById(result.data.colorId);
          const shape = this.getShapeById(result.data.shapeId);
          this.dataList.splice(index, 1, {
            id: result.data.id,
            name: result.data.name,
            color: color,
            shape: shape
          });

          console.log(result);
          this.showOkButton = false;
        } catch (error) {
          console.error("Error updating data:", error);
        }
      } else {
        alert("Please fill in all fields.");
      }
    },

    editItem(index) {
      this.refIndex = index;
      this.showOkButton = true;
      this.name = this.dataList[index].name;

    },

    getColorById(colorId) {
      switch (colorId) {
        case 1: return 'Red';
        case 2: return 'Blue';
        case 3: return 'Yellow';
        case 4: return 'Black';
        case 5: return 'White';
        case 6: return 'Green';
      }
    },

    getShapeById(shapeId) {
      switch (shapeId) {
        case 1: return 'Circle';
        case 2: return 'Triangle';
        case 3: return 'Rectangle';
        case 4: return 'Square';
        case 5: return 'Cone';
        case 6: return 'Sphere';
      }
    },
    takeQuiz() {
      this.$router.push("/quiz");
    },
    // saveDataToLocalStorage() {
    //   localStorage.setItem("dataList", JSON.stringify(this.dataList));
    // }
  },
  // watch: {
  //   dataList: {
  //     handler() {
  //       this.saveDataToLocalStorage();
  //     },
  //     deep: true
  //   }
  // }
};

</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.form {
  width: 50%;
  margin-bottom: 20px;
}

.data-list {
  width: 60%;
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 1.5rem;
}

label {
  display: block;
  font-weight: bold;
}

input[type="text"],
select {
  width: 100%;
  padding: 0.5rem;
  font-size: 1rem;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.btn {
  display: inline-block;
  padding: 0.5rem 1rem;
  font-size: 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn-info {
  background-color: #17a2b8;
  color: #fff;
}

.list-group {
  list-style: none;
  padding: 0;
}

.list-item {
  margin-bottom: 1rem;
  padding: 1rem;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.btn-edit {
  margin-right: 0.5rem;
  background-color: #007bff;
  color: #fff;
}

.btn-delete {
  background-color: #dc3545;
  color: #fff;
}
</style>
