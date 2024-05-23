<template>
  <div class="container" v-if="objects.length > 0">
    <h1>Take Quiz</h1>
    <label>Please answer with True or False</label>
    <br><br>
    <label>Take Care if you didn't answer will be false</label>
    <br><br>
    <div class="image-container">
      <div class="image-wrapper">
        <label>Color</label>
        <img :src="currentObject.color" class="image" alt="Image 1" />
      </div>
      <div class="image-wrapper">
        <label>Shape</label>
        <img :src="currentObject.shape" class="image" alt="Image 2" />
      </div>
    </div>
    <div class="info">
      <label for="name">Name:</label>
      <div class="name">{{ currentObject.name }}</div>

      <div class="buttons">
        <button class="btn btn-true" @click="checkAnswer(true)">True</button>
        <button class="btn btn-false" @click="checkAnswer(false)">False</button>
      </div>
      <br><br>

      <div class="button-row">
        <button class="btn" @click="prevObject" :disabled="currentIndex === 0">Previous</button>
        <button class="btn" @click="nextObject" :disabled="currentIndex === objects.length - 1">Next</button>
      </div>
    </div>

  </div>
  <div v-else>
    Loading...
  </div>
</template>

<script>

import axios from 'axios';

export default {
  data() {
    return {
      currentIndex: 0,
      id: "",
      name: "",
      color: "",
      shape: "",
      objects: [],
      newObject: [],
      score: 0,
      Pass: ""
    };
  },
  created() {
    this.fetchData();
  },
  computed: {
    currentObject() {
      return this.newObject[this.currentIndex];
    }
  },
  methods: {
    async fetchData() {
      try {
        let response = await axios.get("https://localhost:7105/api/Quiz/QuizColorShape");
        console.log(response);
        if (Array.isArray(response.data)) {
          response.data.forEach(obj => {

            this.objects.push({
              name: obj.name,
              color: obj.colorSrc,
              shape: obj.shapeSrc
            });
          });
          this.newObject = this.objects.map(item => ({ ...item }));

          let indexToSwap1 = Math.floor(Math.random() * this.newObject.length);
          let indexToSwap2 = Math.floor(Math.random() * this.newObject.length);


          while (indexToSwap1 === indexToSwap2) {
            indexToSwap2 = Math.floor(Math.random() * this.newObject.length);
          }


          let temp = this.newObject[indexToSwap1].color;
          this.newObject[indexToSwap1].color = this.newObject[indexToSwap2].color;
          this.newObject[indexToSwap2].color = temp;

          // console.log("this is a new list", this.newObject);
        } else {
          console.warn("Received data is not in the expected format.");
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }


    },
    prevObject() {
      if (this.currentIndex > 0) {
        this.currentIndex--;
      }
    },
    nextObject() {
      if (this.currentIndex < this.objects.length - 1) {
        this.currentIndex++;
      }
      else {
        console.log("your score =", this.score);
        if (this.score >= this.objects.length / 2) {
          this.Pass = "you are passed";
          console.log("you are passed");
        }
        else {
          this.Pass = "Game Over";
          console.log("Game Over");
        }
        this.Result(this.score, this.Pass);
      }
    },
    checkAnswer(response) {
      const current = this.currentObject;
      const original = this.objects[this.currentIndex];
      if (
        current.name === original.name &&
        current.color === original.color &&
        current.shape === original.shape
      ) {
        if (response === true) {
          this.score++;
          this.nextObject();
        }
        else {
          this.nextObject();
        }
      }
      else if (
        current.name !== original.name ||
        current.color !== original.color ||
        current.shape !== original.shape
      ) {
        if (response === false) {
          this.score++;
          this.nextObject();
        }
        else {
          this.nextObject();
        }
      }

    },
    async Result(score, pass) {
      try {
        let result = await axios.post("https://localhost:7105/api/Quiz/Result", {
          score: score,
          pass: pass
        });
        console.warn(result);
        this.$router.push("/res");
      } catch (error) {
        console.error("Error in Send data:", error);
      }
    }
  }
};
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.image-container {
  display: flex;
  justify-content: center;
  margin-bottom: 20px;
}

.image-wrapper {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-right: 20px;
}

.image {
  width: 200px;
  height: auto;
}

.info {
  width: 50%;
  text-align: center;
}

label {
  display: block;
  font-weight: bold;
  margin-bottom: 10px;
}

.name {
  font-size: 1.2rem;
  margin-bottom: 20px;
}

.buttons {
  margin-bottom: 10px;
}

.button-row {
  display: flex;
  justify-content: space-around;
}

.btn {
  padding: 0.5rem 1rem;
  font-size: 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn:hover {
  background-color: #eee;
}

.btn-true {
  background-color: #28a745;
  color: #fff;
}

.btn-false {
  background-color: #dc3545;
  color: #fff;
}
</style>
