import { mount } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import ShowMessage from '../src/components/ShowMessage.vue'

describe('ShowMessage.vue', () => {
    it('should render', () => {
        const wrapper = mount(ShowMessage)
        expect(wrapper.html()).toMatchSnapshot()
    })
})
