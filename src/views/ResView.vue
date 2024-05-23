<template>
    <div>
        <div class="final-result">
            <h2>Your Score</h2>
            <p>Score: {{ LastRes.score }}</p>
            <p class="pass">{{ LastRes.pass }}</p>
        </div>
        <div>
            <h1>Old Results</h1>
            <table class="result-table">
                <thead>
                    <tr>
                        <th>Number Of Quizs</th>
                        <th>Score</th>
                        <th>Pass</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(result, index) in results" :key="index">
                        <td>{{ result.id }}</td>
                        <td>{{ result.score }}</td>
                        <td>{{ result.pass }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
import axios from 'axios';

export default {
    data() {
        return {
            results: [],
            LastRes: {
                id: 0,
                score: 0,
                pass: ""
            },
        }
    },
    created() {
        this.fetchData();
    },
    methods: {
        async fetchData() {
            try {
                let response = await axios.get("https://localhost:7105/api/Quiz/getResult");
                console.log(response);
                if (Array.isArray(response.data)) {
                    this.results = response.data.map(obj => ({
                        id: obj.id,
                        score: obj.score,
                        pass: obj.pass
                    }));

                    this.LastRes = this.results[this.results.length - 1];

                    console.log("this is a new list", this.LastRes);
                } else {
                    console.warn("Received data is not in the expected format.");
                }
            } catch (error) {
                console.error("Error fetching data:", error);
            }
        }
    }
}
</script>

<style>
.result-table {
    width: 100%;
    border-collapse: collapse;
}

.result-table th,
.result-table td {
    border: 1px solid #ddd;
    padding: 8px;
    text-align: left;
}

.result-table th {
    background-color: #f2f2f2;
}

.pass {
    text-align: left;
    font-size: 28px; 
    margin-left: 800px; 
    color: blue; 
}
</style>
