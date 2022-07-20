<template>
  <v-navigation-drawer v-model="value" width="500" location="right" temporary>
    <v-card class="mx-auto" flat>
      <v-card-title> Create Item </v-card-title>
      <v-card-subtitle class="d-block">
        <v-form>
          <v-select
            v-model="data.projId"
            :items="projectMasterData"
            label="Select Project"
            variant="outlined"
            item-title="projName"
            item-value="projId"
            density="comfortable"
            @update:modelValue="getProjectDetailMasterData"
          />
          <v-text-field
            :modelValue="props.parentItem?.itemName"
            v-show="parentItem"
            label="Parent Item Name"
            density="comfortable"
            readonly
          />
        </v-form>
      </v-card-subtitle>
      <v-divider></v-divider>
      <v-form class="pa-4 pt-6">
        <v-text-field
          v-model="v$.itemName.$model"
          :error-messages="nameErrors"
          :rules="[ nameErrors.length === 0 ]"
          label="Item Name"
          density="comfortable"
        />
        <!-- <v-combobox
          v-model="data.users"
          :items="assignees"
          chips
          clearable
          label="Assign Users"
          multiple
          solo
        >
          <template v-slot:selection="{ attrs, item, select, selected }">
            <v-chip
              v-bind="attrs"
              :input-value="selected"
              closable
              @click="select"
              @click:close="remove(item)"
            >
              <strong>{{ item.displayName }}</strong>&nbsp;
            </v-chip>
          </template>
        </v-combobox> -->
        <v-select
          v-model="data.users"
          :items="assignees"
          label="Assign Users"
          item-title="displayName"
          item-value="userId"
          density="comfortable"
          chips
          multiple
        />
        <v-select
          v-model="data.projItemTypeId"
          :items="selectedProject.itemTypes"
          label="Item Type"
          item-title="itemTypeName"
          item-value="itemTypeId"
          density="comfortable"
        />
        <v-select
          v-model="data.projPriorityId"
          :items="selectedProject.priorities"
          label="Priority"
          item-title="priorityName"
          item-value="priorityId"
          density="comfortable"
        />
        <v-text-field
          v-model="data.startDate"
          label="Start Date"
          density="comfortable"
        />
        <v-text-field
          v-model="data.endDate"
          label="End Date"
          density="comfortable"
        />
        <v-select
          v-model="data.estimatePoint"
          :items="selectedProject.estimatePoints"
          label="Estimation Points"
          density="comfortable"
        />
      </v-form>
      <v-card-actions style="justify-content: flex-end;">
        <v-btn v-show="!props.item" color="success" flat @click="create">
          Create
        </v-btn>
        <v-btn v-show="props.item"  color="success" flat @click="update">
          Update
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-navigation-drawer>
</template>

<script setup>
import { computed, reactive, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { required } from "@vuelidate/validators";
import useVuelidate from '@vuelidate/core'
import projectApi from "@/api/resources/project";
import sprintApi from "@/api/resources/sprint";
import itemApi from "@/api/resources/item";
import appStore from "@/store/app";
import moment from 'moment'

const { t } = useI18n()
const app = appStore()

const props = defineProps({
  modelValue: Boolean,
  assignees: Array,
  project: Object,
  parentItem: Object,
  item: Object
});
const emit = defineEmits(["update:modelValue", "afterSaved"]);

const projectMasterData = ref([]);
const data = ref({
  id: null,
  projId: null,
  sprintId: null,
  itemName: null,
  projItemTypeId: null,
  projPriorityId: null,
  users: [],
  startDate: new Date().toISOString().substr(0, 10),
  endDate: new Date().toISOString().substr(0, 10),
  point: null,
  description: null,
  duration: null,
  estimatePoint: null
});

const v$ = useVuelidate({
  itemName: { required }
}, data)

const selectedProject = ref({});

const value = computed({
  get() {
    return props.modelValue;
  },
  set(value) {
    emit("update:modelValue", value);
  },
});

const nameErrors = computed(() => {
  const errors = []
  if (!v$.value.itemName.$dirty) return errors
  v$.value.itemName.required.$invalid && errors.push(v$.value.itemName.required.$message)
  return errors
})

watch(value, async (newVal) => {
  if (newVal && projectMasterData.value.length == 0) {
    const resProject = await projectApi.getAll();
    resProject.forEach(p => {
        p.priorities = [];
        p.itemTypes = [];
        p.estimatePoints = [];
    });
    projectMasterData.value = resProject;
  }

  if (newVal) {
    data.value.projId = props.project?.projId
  }

  if (newVal && props.parentItem) {
    data.value.projId = props.parentItem?.projId
    data.value.id = props.parentItem?.id
  }

  if (newVal && props.item) {
    data.value = {
      id: props.item.id,
      projId: props.item.projId,
      sprintId: props.item.sprintId,
      itemName: props.item.itemName,
      projItemTypeId: props.item.projItemTypeId,
      projPriorityId: props.item.projPriorityId,
      users: props.item.ownerId,
      startDate: new Date(props.item.startDate).toISOString().substr(0, 10),
      endDate: new Date(props.item.endDate).toISOString().substr(0, 10),
      description: props.item.description,
      duration: props.item.duration,
      estimatePoint: props.item.estimatePoint
    }
  }

  if (!newVal) {
    data.value = {
      id: null,
      projId: null,
      sprintId: null,
      itemName: null,
      projItemTypeId: null,
      projPriorityId: null,
      users: [],
      startDate: new Date().toISOString().substr(0, 10),
      endDate: new Date().toISOString().substr(0, 10),
      point: null,
      description: null,
      duration: null,
      estimatePoint: null
    }
  }
});

const customStartDateFormatter = computed(() => {
  return data.value.startDate ? moment(data.value.startDate).format("LL") : "";
})
const customEndDateFormatter = computed(() => {
  return data.value.endDate ? moment(data.value.endDate).format("LL") : "";
})

const getProjectDetailMasterData = async () => {
    selectedProject.value = projectMasterData.value.find(p => p.projId == data.value.projId);

    if (selectedProject.value && selectedProject.value.priorities.length === 0) {
        const [ resProjDetail, resSprints ] = await Promise.all([
            projectApi.getProjectDetailAsync(selectedProject.value.projNo),
            sprintApi.search(selectedProject.value.projId)
        ]);

        selectedProject.value.priorities = resProjDetail.projPriorities;
        resProjDetail.projItemTypes.forEach(type => {
          if(type.itemTypeName === "Task" || type.itemTypeName === "Bug")
            selectedProject.value.itemTypes.push(type)
        })
        
        selectedProject.value.estimatePoints = [ 0, 1, 2, 3, 4, 6, 8, 10, 12, 16, 20, 24, 28, 32, 40, 48 ];

        data.value.projItemTypeId = selectedProject.value.itemTypes[0].itemTypeId;
        data.value.projPriorityId = selectedProject.value.priorities[0].priorityId;
        data.value.estimatePoints = selectedProject.value.estimatePoints[0];

        const activeSprint = resSprints.find(s => s.sprintType === 2);
        if (activeSprint) {
          data.value.sprintId = activeSprint.sprintId
        }
    }
};

const create = async () => {
  v$.value.$touch()
  const isValidateError = await v$.value.$validate()

  if (isValidateError) {
    await app.load(async () => {
      const parameter = Object.assign(data.value, {
        name: v$.value.itemName.$model
      })

      const res = data.value.id === null ? await itemApi.create(data.value) : await itemApi.createSubItem(data.value)

      res.projId = data.value.projId
      res.projName = selectedProject.value.projName
      res.projNo = selectedProject.value.projNo
      res.projItemTypeName = selectedProject.value.itemTypes.find(t => t.itemTypeId === data.value.projItemTypeId).itemTypeName;
      res.sprintId = data.value.sprintId
      emit("afterSaved", res);
    })
  }
}

const update = async () => {
  await app.load(async () => {
    await itemApi.update(data.value)

    emit("afterSaved", data.value);
  })
}

const remove = (item) => {
  assignees.splice(assignees.indexOf(item), 1)
}
</script>