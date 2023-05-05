<script>
import * as bootstrap from "bootstrap";
import { useUrlStore } from "@/stores/url";
import { useLoginStore } from "@/stores/login";
import { storeToRefs } from "pinia";
const storeUrl = useUrlStore();
const storeLogin = useLoginStore();

class Student {
  constructor(
    id = 0,
    name = null,
    year = null
  ) {
    this.id = id;
    this.name = name;
    this.year = year;
  }
}

export default {
  data() {
    return {
      storeUrl,
      storeLogin,
      Students: [],
      currentStudentId: null,
      edittableStudent: new Student(),
      modal: null,
      form: null,
      state: "view",
      currentId: null,
      yesNoShow: false,
      query: null,
    };
  },
  mounted() {
    this.getStudents();
    this.modal = new bootstrap.Modal(document.getElementById("modalStudent"), {
      keyboard: false,
    });
  },
  methods: {
    async getStudents() {
      let url = this.storeUrl.urlStudents;
      const config = {
        method: "GET",
        headers: {
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      const data = await response.json();
      this.Students = data.data;
    },
    async getStudentById(id) {
      let url = `${this.storeUrl.urlStudentById}/${id}`;
      const config = {
        method: "GET",
        headers: {
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      const data = await response.json();
      this.edittableStudent = data.data;
    },
    async deleteStudent(id) {
      let url = `${this.storeUrl.urlStudents}/${id}`;
      const config = {
        method: "DELETE",
        headers: {
          "content-type": "application/json",
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
      };
      const response = await fetch(url, config);
      this.getStudents();
    },
    async postStudent() {
      let url = this.storeUrl.urlStudents;
      const body = JSON.stringify(this.edittableStudent);
      const config = {
        method: "POST",
        headers: {
          "content-type": "application/json",
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
        body: body,
      };
      const response = await fetch(url, config);
      this.getStudents();
    },
    async putStudent() {
      const id = this.edittableStudent.id;
      let url = `${this.storeUrl.urlStudents}/${id}`;
      const body = JSON.stringify(this.edittableStudent);
      const config = {
        method: "PUT",
        headers: {
          "content-type": "application/json",
          Authorization: `Bearer ${this.storeLogin.accessToken}`,
        },
        body: body,
      };
      const response = await fetch(url, config);
      this.getStudents();
    },
    onClikRow(id) {
      this.currentId = id;
    },
    onClickNew() {
      this.state = "new";
      this.currentStudentId = null;
      this.edittableStudent = new Student();
      this.modal.show();
    },
    onClickDelete(id) {
      this.state = "delete";
      this.deleteStudent(id);
      this.currentStudentId = null;
    },
    onClickEdit(id) {
      this.state = "edit";
      this.getStudentById(id);
      this.modal.show();
    },
    onClickDeleteOK() {
      this.deleteStudent(this.currentStudentId);
      this.yesNoShow = false;
    },
    onClickDeleteCancel() {
      this.yesNoShow = false;
    },
    onClickCancel() {
      this.edittableStudent = new Student();
      this.modal.hide();
    },
    onClickSave() {
      this.form.classList.add("was-validated");
      if (this.form.checkValidity()) {
        if (this.state == "edit") {
          //put
          this.putStudent();
          this.modal.hide();
        } else if (this.state == "new") {
          //post
          this.postStudent();
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
    filteredStudent:function(){
      if (!this.query) {
        return this.Students;
      }else {
        let what = new RegExp(this.query, "gi");
        var self = this;
        return self.Students.filter(function(val){
          return val.name.match(what);
        })
      }
    },
    stateTitle() {
      if (this.state === "new") {
        return "Új diák bevitele";
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
    <div id="searchbarcontainer" class="w-50">
      <h1>Diákok</h1>

      <form class="d-flex" role="search">
        <input class="form-control me-2" v-model="query" type="input" placeholder="Search" aria-label="Search" />
      </form>

    </div>

    <div id="datatable" class="w-50">
      <table class="table table-dark table-striped table-hover">
        <thead>
          <tr>
            <th>Név</th>
            <th>Osztály</th>
            <th></th>
            <th><button type="button" class="btn btn-success" data-bs-toggle="modalStudent" data-bs-target="modalStudent"
                @click="onClickNew()"><i class="bi bi-person-fill-add"></i></button></th>
          </tr>
        </thead>
        <tbody>
          <tr  v-for="(student, index) in filteredStudent" :key="`student${index}`"
          :class="currentRowBackground(student.id)"
          @click="onClikRow(student.id)">
            <td>{{ student.name }}</td>
            <td>{{ student.year }}</td>
            <td><button type="button" class="btn btn-success" @click="onClickEdit(student.id)"><i
                  class="bi bi-pen"></i></button></td>
            <td><button type="button" class="btn btn-danger" @click="onClickDelete(student.id)"><i
                  class="bi bi-x-circle"></i></button></td>
          </tr>
        </tbody>
      </table>
    </div>
    
    <!-- Modal -->
    <div class="modal fade" id="modalStudent" tabindex="-1" aria-labelledby="modalStudentModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h1 class="modal-title fs-5" id="exampleModalLabel">{{ stateTitle }}</h1>
            <button type="button" class="btn-close" aria-label="Close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <form>
              <!-- Students name -->
              <div class="mb-3">
                <label for="name" class="form-label">Diák neve</label>
                <input type="text" class="form-control" id="name" required v-model="edittableStudent.name">
                <div class="invalid-feedback">Adjon meg egy nevet</div>
              </div>
              <!-- Students year -->
              <div class="mb-3">
                <label for="year" class="form-label">Osztály</label>
                <input type="number" class="form-control" id="year" required v-model="edittableStudent.year">
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

    <!-- delete modal component -->
    <!-- <Menu></Menu>
    <YesNo v-if="yesNoShow" yesNoTitle="Diák törlés" yesNoMessage="Valóban törölni akarja ezt a diákot?"
      @yes="onClickDeleteOK()" @no="onClickDeleteCancel()"></YesNo> -->
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

#searchbarcontainer {
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
