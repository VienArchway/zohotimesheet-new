<template>
  <v-navigation-drawer v-model="value" width="500" location="right" temporary>
    <v-card class="mx-auto" height="100%">
      <v-card-title> Create Item </v-card-title>
      <v-card-subtitle>
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
      </v-card-subtitle>
      <v-divider></v-divider>
      <v-form>
        <v-list>
          <v-list-item-group>
            <v-list-item>
              <v-text-field
                v-model="data.name"
                label="Item Name"
                density="comfortable"
              />
            </v-list-item>
            <v-list-item>
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
            </v-list-item>
            <v-list-item>
              <v-select
                v-model="data.projItemTypeId"
                :items="selectedProject.itemTypes"
                label="Item Type"
                item-title="itemTypeName"
                item-value="itemTypeId"
                density="comfortable"
              />
            </v-list-item>
            <v-list-item>
              <v-select
                v-model="data.projPriorityId"
                :items="selectedProject.prorities"
                label="Priority"
                item-title="priorityName"
                item-value="priorityId"
                density="comfortable"
              />
            </v-list-item>
            <v-list-item>
              <v-text-field
                v-model="data.startDate"
                label="Start Date"
                density="comfortable"
              />
            </v-list-item>
            <v-list-item>
              <v-text-field
                v-model="data.endDate"
                label="End Date"
                density="comfortable"
              />
            </v-list-item>
            <v-list-item>
              <v-select
                :items="projectMasterData"
                label="Estimation Points"
                item-title="projName"
                item-value="projId"
                density="comfortable"
              />
            </v-list-item>
          </v-list-item-group>
        </v-list>
      </v-form>
      <v-card-actions style="justify-content: flex-end;">
        <v-btn color="success" flat @click="create">
          Create
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-navigation-drawer>
</template>

<script setup>
import { computed, reactive, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import projectApi from "@/api/resources/project";
import sprintApi from "@/api/resources/sprint";
import itemApi from "@/api/resources/item";
import appStore from "@/store/app";

const { t } = useI18n()
const app = appStore()

const props = defineProps({
  modelValue: Boolean,
  assignees: Array
});
const emit = defineEmits(["update:modelValue", "afterCreate"]);

const projectMasterData = ref([]);
const data = ref({
  projId: null,
  sprintId: null,
  name: null,
  projItemTypeId: null,
  projPriorityId: null,
  users: [],
  startDate: null,
  endDate: null
});
const selectedProject = ref({});

const value = computed({
  get() {
    return props.modelValue;
  },
  set(value) {
    emit("update:modelValue", value);
  },
});

watch(value, async (newVal) => {
  if (newVal && projectMasterData.value.length == 0) {
    const resProject = await projectApi.getAll();
    resProject.forEach(p => {
        p.prorities = [];
        p.itemTypes = [];
    });
    projectMasterData.value = resProject;
  }
});

const getProjectDetailMasterData = async () => {
    selectedProject.value = projectMasterData.value.find(p => p.projId == data.value.projId);

    if (selectedProject.value && selectedProject.value.prorities.length === 0) {
        const [ resPriotiries, resItemTypes, resSprints ] = await Promise.all([
            projectApi.getProjectPriorityAsync(selectedProject.value.projId),
            projectApi.getProjectItemTypeAsync(selectedProject.value.projId),
            sprintApi.search(selectedProject.value.projId)
        ]);

        selectedProject.value.prorities = resPriotiries;
        selectedProject.value.itemTypes = resItemTypes;
        
        const activeSprint = resSprints.find(s => s.sprintType === 2);
        if (activeSprint) {
          data.value.sprintId = activeSprint.sprintId
        }
    }
};

const create = async () => {
  await app.load(async () => {
    const res = await itemApi.create(data.value)

    debugger
    res.projId = data.value.projId
    res.projName = selectedProject.value.projName
    res.projNo = selectedProject.value.projNo
    res.projItemTypeName = selectedProject.value.itemTypes.find(t => t.itemTypeId === data.value.projItemTypeId).itemTypeName;
    res.sprintId = data.value.sprintId
    emit("afterCreate", res);
  })
}

const remove = (item) => {
  assignees.splice(assignees.indexOf(item), 1)
}
</script>