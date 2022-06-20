<script setup>
import { useI18n } from 'vue-i18n'
import { getZohoUserDisplayName } from '@/api/resources/zohoToken'
import { onMounted, ref } from 'vue'

const { t } = useI18n()
const userName = ref(null)

onMounted(async () => {
  userName.value = await getZohoUserDisplayName()
})

</script>

<template>
  <v-app-bar app color="teal" dark>
    <img src="@/assets/images/zohoImage.png" alt="logo" width="48" height="36" />
    <v-menu>
      <template v-slot:activator="{ props }">
        <v-btn color="primary" v-bind="props">
          {{ t("transfer") }}
        </v-btn>
      </template>

      <v-list>
        <v-list-item to="/">{{ t("manually") }}</v-list-item>
        <v-list-item to="schedule">{{ t("schedule") }}</v-list-item>
      </v-list>
    </v-menu>
    <v-btn to="timesheet">
      {{ t("timeSheet") }}
    </v-btn>
    <v-menu>
      <template v-slot:activator="{ props }">
        <v-btn v-bind="props">
          {{ t("powerBI") }}
        </v-btn>
      </template>
      <v-list>
        <v-list-item to="azuredatalake">{{ t("data") }}</v-list-item>
        <v-list-item href="https://app.powerbi.com/groups/4f7c2e73-a46b-4a81-bdc8-57bb761d663c/list/datasets" target="_blank">
          {{ t("dataset") }}
        </v-list-item>
      </v-list>
    </v-menu>
    <v-btn href="https://projectmanagementstrage.z11.web.core.windows.net/" target="_blank">
      {{ t("projectmanagement") }}
    </v-btn>
    <v-btn to="/webhook">
      {{ t("webhook") }}
    </v-btn>
    <v-spacer></v-spacer>

    <v-avatar size="36">
      <img src="https://contacts.zoho.com/file?fs=thumb&exp=600" alt="user-avatar"/>
    </v-avatar>
    <div class="ml-2 mr-2">{{ userName }}</div>
  </v-app-bar>
</template>