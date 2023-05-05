<script>
import * as bootstrap from "bootstrap";
import { useUrlStore } from "@/stores/url";
import { useLoginStore } from "@/stores/login";
import { storeToRefs } from "pinia";
const storeUrl = useUrlStore();
const storeLogin = useLoginStore();

class Data {
  constructor(
    id = 0,
    writer = null,
    title = null,
    year = null,
    sId = 0,
    opusid = 0,
    price = null,
    acquisition = null,
  ) {
    this.id = id;
    this.writer = writer;
    this.title = title;
    this.year = year;
    this.sId = sId;
    this.opusid = opusid;
    this.price = price;
    this.acquisition = acquisition;
  }
}

export default {
  data() {
    return {
      storeUrl,
      storeLogin,
      Data: [],
      currentDataId: null,
      edittableData: new Data(),
      modal: null,
      form: null,
      state: "view",
      currentId: null,
      yesNoShow: false,
      query: null,
    };
  },
  mounted() {
    this.getSpecimenOpusJoin();
    this.getSpecimen();
    this.modal = new bootstrap.Modal(document.getElementById("modalSpecimen"), {
      keyboard: false,
    });
  },
  methods: {
    async getSpecimenOpusJoin() {
      let url = this.storeUrl.urlSpecimenOpusJoin;
      const config = {
        method: "GET",
        headers: {
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      const data = await response.json();
      this.Data = data.data;
    },
    async getSpecimen() {
      let url = this.storeUrl.urlSpecimen;
      const config = {
        method: "GET",
        headers: {
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      const data = await response.json();
      this.Data = data.data;
    },
    async getDataById(id) {
      let url = `${this.storeUrl.urlSpecimen}/${id}`;
      const config = {
        method: "GET",
        headers: {
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      const data = await response.json();
      this.edittableData = data.data;
    },
    async getSpecimenById(id) {
      let url = `${this.storeUrl.urlSpecimenOpusJoin}/${id}`;
      const config = {
        method: "GET",
        headers: {
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      const data = await response.json();
      this.edittableData = data.data;
    },
    async deleteSpecimen(id) {
      let url = `${this.storeUrl.urlSpecimen}/${id}`;
      const config = {
        method: "DELETE",
        headers: {
          "content-type": "application/json",
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      this.getSpecimenOpusJoin();
    },
    async postSpecimen() {
      let url = this.storeUrl.urlSpecimen;
      const body = JSON.stringify(this.edittableData);
      const config = {
        method: "POST",
        headers: {
          "content-type": "application/json",
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
        body: body,
      };
      const response = await fetch(url, config);
      this.getSpecimenOpusJoin();
    },
    async putSpecimen() {
      const id = this.edittableData.id;
      let url = `${this.storeUrl.urlSpecimen}/${id}`;
      const body = JSON.stringify(this.edittableData);
      const config = {
        method: "PUT",
        headers: {
          "content-type": "application/json",
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
        body: body,
      };
      const response = await fetch(url, config);
      this.getSpecimenOpusJoin();
    },
    onClikRow(id) {
      this.currentId = id;
    },
    onClickNew() {
      this.state = "new";
      this.currentDataId = null;
      this.edittableData = new Data();
      this.modal.show();
    },
    onClickDelete(id) {
      this.state = "delete";
      this.deleteSpecimen(id);
      this.currentDataId = null;
    },
    onClickEdit(id) {
      this.state = "edit";
      this.getSpecimenById(id);
      this.modal.show();
    },
    onClickDeleteOK() {
      this.deleteSpecimen(this.currentDataId);
      this.yesNoShow = false;
    },
    onClickDeleteCancel() {
      this.yesNoShow = false;
    },
    onClickCancel() {
      this.edittableData = new Data();
      this.modal.hide();
    },
    onClickSave() {
      this.form.classList.add("was-validated");
      if (this.form.checkValidity()) {
        if (this.state == "edit") {
          //put
          this.putSpecimen();
          this.modal.hide();
        } else if (this.state == "new") {
          //post
          this.postSpecimen();
          this.modal.hide();
        }
        this.modal.hide();
      }
    },
    currentRowBackground(id) {
      return this.currentId == id ? "my-bg-current-row" : "";
    },
  },
  computed: {
    filteredData:function(){
      if (!this.query) {
        return this.Data;
      }else {
        let what = new RegExp(this.query, "gi");
        var self = this;
        return self.Data.filter(function(val){
          return val.title.match(what) || val.acquisition.match(what);
        })
      }
    },
    stateTitle() {
      if (this.state === "new") {
        return "Új példány bevitele";
      } else if (this.state === "edit") {
        console.log('Módosít');
        return "Példány módosítása";
      }
    },
  },
};
</script>

<template>
  <div>
    <div id="searchbarcontainer" class="w-75">
      <h1>Példányok</h1>

      <form class="d-flex" role="search">
        <input class="form-control me-2" v-model="query" type="input" placeholder="Search" aria-label="Search" />
      </form>

    </div>

    <div id="datatable" class="w-75">
      <table class="table table-dark table-striped table-hover">
        <thead>
          <tr>
            <th>Cím</th>
            <th>Beszerzés</th>
            <th>Ár</th>
            <th><button type="button" class="btn btn-success" data-bs-toggle="modalSpecimen" data-bs-target="#modalSpecimen"
              :class="{ disabled: !storeLogin.loginSuccess}"
              @click="onClickNew()"><i class="bi bi-file-earmark-plus-fill"></i></button></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(specimen, index) in filteredData" :key="`specimen${index}`"
        :class="currentRowBackground(specimen.id)"
          @click="onClikRow(specimen.id)">
            <td>{{ specimen.title }}</td>
            <td>{{ specimen.acquisition }}</td>
            <td>{{ specimen.price }} Ft</td>
            <td></td>
            <!-- <td><button type="button" class="btn btn-success" @click="onClickEdit(specimen.id)"><i
                  class="bi bi-pen"></i></button></td>
            <td><button type="button" class="btn btn-danger" @click="onClickDelete(specimen.id)"><i
                  class="bi bi-x-circle"></i></button></td> -->
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="modalSpecimen" tabindex="-1" aria-labelledby="modalSpecimenModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h1 class="modal-title fs-5" id="exampleModalLabel">{{ stateTitle }}</h1>
            <button type="button" class="btn-close" aria-label="Close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <form>
              <!-- Opus id -->
              <div class="mb-3">
                <label for="opusid" class="form-label">Könyv id</label>
                <input type="number" class="form-control" id="opusid" required v-model="edittableData.opusid">
                <div class="invalid-feedback">Please enter an opus id</div>
              </div>
              <!-- Price -->
              <div class="mb-3">
                <label for="price" class="form-label">Ár</label>
                <input type="number" class="form-control" id="price" required v-model="edittableData.price">
              </div>
              <!-- Acquisition -->
              <div class="mb-3">
                <label for="acquisition" class="form-label">Beszerzés</label>
                <input type="text" class="form-control" id="acquisition" required v-model="edittableData.acquisition">
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" @click="onClickCancel()">Cancel</button>
            <button type="button" class="btn btn-primary"  @click="onClickSave()">Save</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
h1 {
  color: white;
  text-align: center;
}

#datatable{
  margin: auto;
  height: 80vh;
  overflow-y: scroll;
}

#searchbarcontainer{
  margin: auto;
  margin-bottom: 5px;
}

#exampleModalLabel {
  color: rgb(91, 90, 90);
}

label.form-label {
  color: rgb(91, 90, 90);
}

</style>