import { render, screen } from '@testing-library/vue'
import ExampleProp from '../../src/components/ExampleProp.vue'

describe('ExampleProp.vue', () => {
    it('h1 message here show correctly', async () => {
        render(ExampleProp)
        
        const text = await screen.findByText('Message here')
        expect(text.nodeName).toBe("H1")
        expect(text.innerHTML).toBe("Message here")
    })
    
    it('when props is empty', async () => {
        render(ExampleProp, { props: {} })
        
        const props = await screen.getByTestId("prop-msg")
        expect(props.innerHTML).toBe('')
        expect(props.nodeName).toEqual('SPAN')
    })

    it('when props have value', async () => {
        var propMsg = 'test-prop-message'
        render(ExampleProp, {
            props: { message: propMsg }
        })

        const props = await screen.getByTestId("prop-msg")
        expect(props.innerHTML).toBe(propMsg)
        expect(props.nodeName).toEqual('SPAN')
    })
})