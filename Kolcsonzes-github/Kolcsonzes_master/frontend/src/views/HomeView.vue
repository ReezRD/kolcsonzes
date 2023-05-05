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
  ) {
    this.id = id;
    this.writer = writer;
    this.title = title;
    this.year = year;
  }
}

class OpusSpecimenData {
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
      OpusSpecimenData: [],
      currentDataId: null,
      edittableData: new Data(),
      edittableOpusSpecimenData: new OpusSpecimenData(),
      form: null,
      state: "view",
      currentId: null,
      query: null,
    };
  },
  mounted() {
    this.getOpus();
    this.getOpusSpecimen();
    this.modal = new bootstrap.Modal(document.getElementById("modalBooks"), {
      keyboard: false,
    });
    this.form = document.querySelector(".needs-validation");
  },
  methods: {
    async getOpus() {
      let url = this.storeUrl.urlOpus;
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
    async getOpusSpecimen() {
      let url = this.storeUrl.urlOpusSpecimen;
      const config = {
        method: "GET",
        headers: {
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      const data = await response.json();
      this.OpusSpecimenData = data.data;
    },
    async getOpusById(id) {
      let url = `${this.storeUrl.urlOpus}/${id}`;
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
    async getOpusSpecimenById(id) {
      let url = `${this.storeUrl.urlOpusSpecimen}/${id}`;
      const config = {
        method: "GET",
        headers: {
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      const data = await response.json();
      this.edittableOpusSpecimenData = data.data;
    },
    async postOpus() {
      let url = this.storeUrl.urlOpus;
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
      this.getOpus();
    },
    onClikRow(id) {
      this.currentId = id;
    },
    currentRowBackground(id) {
      return this.currentId == id ? "my-bg-current-row" : "";
    },
    onClickNew() {
      this.state = "new";
      this.currentDataId = null;
      this.edittableData = new Data();
      this.modal.show();
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
          this.postOpus();
          // this.modal.hide();
        }
        this.modal.hide();
        //frissíti a taxisok listáját
        this.getOpusSpecimen()
      }
    },
  },
  computed: {
    filteredData:function(){
      if (!this.query) {
        return this.OpusSpecimenData;
      }else {
        let what = new RegExp(this.query, "gi");
        var self = this;
        return self.Data.filter(function(val){
          return val.writer.match(what) || val.title.match(what);
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
      <h1>Könyveink</h1>

      <form class="d-flex" role="search">
        <input class="form-control me-2" v-model="query" type="input" placeholder="Search" aria-label="Search" />
      </form>

    </div>
    

    <div id="datatable" class="w-75">
      <table class="table table-dark table-striped table-hover table-sm">
        <thead>
          <tr>
            <th>Író</th>
            <th>Cím</th>
            <th>Ár</th>
            <th><button type="button" class="btn btn-success" data-bs-toggle="modalLoaning" data-bs-target="#modalLoaning"
              :class="{disabled: !storeLogin.loginSuccess}"
              @click="onClickNew()"><i class="bi bi-file-earmark-plus-fill"></i></button></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(opus, index) in filteredData" :key="`opus${index}`">
            <td>{{ opus.writer }}</td>
            <td>{{ opus.title }}</td>
            <td>{{ opus.price }} Ft</td>
            <td></td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="modalBooks" tabindex="-1" aria-labelledby="modalBooksModalLabel" aria-hidden="true">
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
                <label for="writer" class="form-label">Író</label>
                <input type="text" class="form-control" id="writer" required v-model="edittableData.writer">
                <div class="invalid-feedback">Író neve</div>
              </div>
              <!-- Student Id -->
              <div class="mb-3">
                <label for="title" class="form-label">Cím</label>
                <input type="text" class="form-control" id="title" required v-model="edittableData.title">
                <div class="invalid-feedback">Cím</div>
              </div>
              <!-- Away -->
              <div class="mb-3">
                <label for="year" class="form-label">Év</label>
                <input type="text" class="form-control" id="year" required v-model="edittableData.year">
                <div class="invalid-feedback">Év</div>
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

#datatable {
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
