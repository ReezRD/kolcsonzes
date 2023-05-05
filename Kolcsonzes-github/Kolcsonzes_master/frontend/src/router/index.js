import { createRouter, createWebHistory } from "vue-router";


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: () => import("../views/HomeView.vue"),
      meta: {
        requiresAuth: false,
        title: "Home / Books",
      },
    },
    {
      path: "/specimen",
      name: "specimen",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/SpecimenView.vue"),
      meta: {
        requiresAuth: false,
        title: "Specimen / Books",
      },
    },
    {
      path: "/loanings",
      name: "loanings",
      component: () => import("../views/LoaningView.vue"),
      meta: {
        requiresAuth: true,
        title: "Loanings / Books",
      },
    },
    {
      path: "/students",
      name: "students",
      component: () => import("../views/StudentView.vue"),
      meta: {
        requiresAuth: true,
        title: "Students / Books",
      },
    },
    {
      path: "/geturebook",
      name: "geturebook",
      component: () => import("../views/GetUreBookView.vue"),
      meta: {
        requiresAuth: true,
        title: "GetUreBook / Books",
      },
    },
    {
      path: "/login",
      name: "login",
      component: () => import("../views/LoginView.vue"),
      meta: {
        requiresAuth: false,
        title: "Login / Books",
      },
    },
    {
      path: "/:pathMatch(.*)*",
      name: "PageNotFound",
      component: () => import("../views/404View.vue"),
      meta: {
        requiresAuth: false,
        title: "404 / Books",
      },
    },
  ],
});

export default router;
