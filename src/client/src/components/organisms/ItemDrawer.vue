<template>
  <v-navigation-drawer v-model="value" width="500" location="right" temporary>
    <v-card class="mx-auto" height="100%">
      <v-card-title> Create Item </v-card-title>
      <v-card-subtitle>
        <v-select
          v-model="selectedProjectId"
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
              <v-text-field label="Item Name" density="comfortable" />
            </v-list-item>
            <v-list-item>
              <v-select
                :items="projectMasterData"
                label="Epic"
                item-title="projName"
                item-value="projId"
                density="comfortable"
              />
            </v-list-item>
            <v-list-item>
              <v-select
                :items="selectedProject.itemTypes"
                label="Item Type"
                item-title="itemTypeName"
                item-value="itemTypeId"
                density="comfortable"
              />
            </v-list-item>
            <v-list-item>
              <v-select
                :items="selectedProject.prorities"
                label="Priority"
                item-title="priorityName"
                item-value="priorityId"
                density="comfortable"
              />
            </v-list-item>
            <v-list-item>
              <v-text-field label="Start Date" density="comfortable" />
            </v-list-item>
            <v-list-item>
              <v-text-field label="End Date" density="comfortable" />
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
    </v-card>
  </v-navigation-drawer>
</template>

<script setup>
import { computed, reactive, ref, watch } from "vue";
import projectApi from "@/api/resources/project";

const props = defineProps(["modelValue"]);
const emit = defineEmits(["update:modelValue"]);

const projectMasterData = ref([]);
const selectedProjectId = ref(null);
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
    selectedProject.value = projectMasterData.value.find(p => p.projId == selectedProjectId.value);

    if (selectedProject.value && selectedProject.value.prorities.length === 0) {
        const [ resPriotiries, resItemTypes ] = await Promise.all([
            projectApi.getProjectPriorityAsync(selectedProject.value.projId),
            projectApi.getProjectItemTypeAsync(selectedProject.value.projId),
        ]);

        selectedProject.value.prorities = resPriotiries;
        selectedProject.value.itemTypes = resItemTypes;
        debugger
    }
};
</script>