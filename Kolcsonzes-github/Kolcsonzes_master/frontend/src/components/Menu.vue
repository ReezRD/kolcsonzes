<template>
  <nav class="navbar sticky-top navbar-expand-md navbar-dark bg-dark">
    <div class="container-fluid">
      <router-link class="navbar-brand" to="/" @click="onClickMenu(1)"><i class="bi bi-house-fill"></i></router-link>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent"
        aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse collapse w-100 order-1 order-md-0 dual-collapse2" id="navbarSupportedContent">
        <ul class="navbar-nav navbar-left me-auto mb-2 mb-lg-0">
          <li class="nav-item">
            <router-link class="nav-link" to="/Specimen" :class="{ active: menuState === 3 }"
              @click="onClickMenu(3)">Példányok</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/Loanings" :class="{ disabled: !storeLogin.loginSuccess }"
              @click="onClickMenu(5)">Kölcsönzések</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/Students" :class="{ disabled: !storeLogin.loginSuccess }"
            @click="onClickMenu(5)">Diákok</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/GetUreBook" :class="{ disabled: !storeLogin.loginSuccess }"
              @click="onClickMenu(5)">Kölcsönzés kereső</router-link>
          </li>
        </ul>
        <ul class="navbar-nav navbar-right ml-auto">
          <li class="nav-item ms-auto" v-if="!storeLogin.loginSuccess">
            <router-link class="nav-link" to="/login">Login</router-link>
          </li>
          <li class="nav-item" v-if="storeLogin.loginSuccess" @click="logout()">
            <router-link class="nav-link" to="/login">Logout ({{ storeLogin.userName }})</router-link>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { useUrlStore } from "@/stores/url";
import { useLoginStore } from "@/stores/login";
const storeUrl = useUrlStore();
const storeLogin = useLoginStore();

const msg = "helo";
let menuState = null;
async function logout() {
  console.log("logout");
  const urlLogout = storeUrl.urlLogout;
  const body = {
    token: storeLogin.refreshToken,
  };
  const config = {
    method: "DELETE",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify(body),
  };
  const response = await fetch(urlLogout, config);
  storeLogin.clearLogin();
}

function onClickMenu(number) {
  this.menuState = number
}

</script>

<style>

nav  {
  position: sticky;
  top: 0px;
  width: 100%;
}
</style>