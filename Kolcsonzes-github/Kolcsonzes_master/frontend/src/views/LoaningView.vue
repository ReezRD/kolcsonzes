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
    name = null,
    specimentid = 0,
    studentid = 0,
    away = null,
    back = null
  ) {
    this.id = id;
    this.writer = writer;
    this.title = title;
    this.year = year;
    this.sId = sId;
    this.opusid = opusid;
    this.price = price;
    this.acquisition = acquisition;
    this.name = name;
    this.specimentid = specimentid;
    this.studentid = studentid;
    this.away = away;
    this.back = back;
  }
}

export default {
  data() {
    return {
      storeUrl,
      storeLogin,
      Data: [],
      // LoaningData: [],
      currentDataId: null,
      edittableData: new Data(),
      modal: null,
      form: null,
      currentBookId: null,
      state: "view",
      currentId: null,
      yesNoShow: false,
      query: null,
    };
  },
  mounted() {
    this.getEverything();
    // this.getLoaning();
    this.modal = new bootstrap.Modal(document.getElementById("modalLoaning"), {
      keyboard: false,
    });

    this.form = document.querySelector(".needs-validation");
  },
  methods: {
    async getEverything() {
      let url = this.storeUrl.urlEverything;
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
    // async getLoaning() {
    //   let url = this.storeUrl.urlLoaning;
    //   const config = {
    //     method: "GET",
    //     headers: {
    //       Authorization: `Bearer ${this.storeLogin.accessToken}`,
    //     },
    //   };
    //   const response = await fetch(url, config);
    //   const data = await response.json();
    //   this.Data = data.data;
    // },
    async getEverythingById(id) {
      let url = `${this.storeUrl.urlEverything}/${id}`;
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
    async postLoaning() {
      let url = this.storeUrl.urlLoaning;
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
      this.getEverything();
    },
    async deleteEverything(id) {
      let url = `${this.storeUrl.urlEverything}/${id}`;
      const config = {
        method: "DELETE",
        headers: {
          "content-type": "application/json",
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      this.getEverything();
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
      this.deleteEverything(id);
      this.currentId = null;
    },
    onClickDeleteOK() {
      this.deleteEverything(this.currentDataId);
      this.yesNoShow = false;
    },
    onClickDeleteCancel() {
      this.yesNoShow = false;
    },
    onClickCancel() {
      this.currentDataId = new Data();
      this.modal.hide();
    },
    onClickSave() {
      this.form.classList.add("was-validated");
      if (this.form.checkValidity()) {
        if (this.state == "new") {
          //put
          this.postLoaning();
          this.modal.hide();
        }
        this.modal.hide();
        //frissíti a taxisok listáját
        this.getEverything();
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
          return val.name.match(what) || val.title.match(what) || val.away.match(what) || val.back.match(what);
        })
      }
    },
    stateTitle() {
      if (this.state === "new") {
        return "Új kölcsönzés bevitele";
      } else if (this.state === "edit") {
        console.log('Módosít');
        return "Diák módosítása";
      }
    },
  },
};
</script>

<template>
  <div>
    <div id="searchbarcontainer" class="w-75">
      <h1>Kölcsönzések</h1>

      <form class="d-flex" role="search">
        <input class="form-control me-2" v-model="query" type="input" placeholder="Search" aria-label="Search" />
      </form>

    </div>

    <div id="datatable" class="w-75 ">
      <table class="table table-dark table-striped table-hover">
        <thead>
          <tr>
            <th>Név</th>
            <th>Cím</th>
            <th>El</th>
            <th>Vissza</th>
            <th></th>
            <th><button type="button" class="btn btn-success" data-bs-toggle="modalLoaning" data-bs-target="#modalLoaning"
              @click="onClickNew()"><i class="bi bi-file-earmark-plus-fill"></i></button></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(loaning, index) in filteredData" :key="`loaning${index}`"
          :class="currentRowBackground(loaning.id)"
          @click="onClikRow(loaning.id)">
            <td>{{ loaning.name }}</td>
            <td>{{ loaning.title }}</td>
            <td>{{ loaning.away }}</td>
            <td>{{ loaning.back }}</td>
            <td></td>
            <td></td>
            <!-- <button type="button" class="btn btn-danger" @click="onClickDelete(loaning.id)"><i class="bi bi-x-circle"></i></button> -->
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="modalLoaning" tabindex="-1" aria-labelledby="modalLoaningModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h1 class="modal-title fs-5" id="exampleModalLabel">{{ stateTitle }}</h1>
            <button type="button" class="btn-close" aria-label="Close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <form>
              <!-- Specimen id -->
              <div class="mb-3">
                <label for="specimentid" class="form-label">Specimen Id</label>
                <input type="number" min="1" class="form-control" id="specimentid" required v-model="edittableData.specimentid">
                <div class="invalid-feedback">Please enter the specimen id</div>
              </div>
              <!-- Student Id -->
              <div class="mb-3">
                <label for="studentid" class="form-label">Student Id</label>
                <input type="number" min="1" class="form-control" id="studentid" required v-model="edittableData.studentid">
              </div>
              <!-- Away -->
              <div class="mb-3">
                <label for="away" class="form-label">Away</label>
                <input type="text" class="form-control" id="away" required v-model="edittableData.away">
              </div>
              <!-- Back -->
              <div class="mb-3">
                <label for="back" class="form-label">Back</label>
                <input type="text" class="form-control" id="back" required v-model="edittableData.back">
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