// src/router/index.js
import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import AboutView from "../views/AboutView.vue";
import ResView from "../views/ResView.vue";

const routes = [
  { path: "/", component: HomeView },
  { path: "/quiz", component: AboutView, props: true },
  { path: "/res", component: ResView, props: true },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
