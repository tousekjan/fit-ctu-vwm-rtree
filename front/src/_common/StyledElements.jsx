import styled from 'styled-components';
import { Layout } from 'antd';

export const ContentContainer = styled.div`
  background: #fff;
  padding: 2em;
  min-height: 17em;

  @media (max-width: 500px) {
    padding: 1.5em;
  }
`;

export const WideContentContainer = styled.div`
  background: #fff;
  padding: 3em;
  min-height: 17em;

  @media (max-width: 500px) {
    padding: 2em;
  }
`;

export const ContentLayout = styled(Layout.Content)`
  width: 60%;
  margin: auto;
  margin-top: 3.5em;

  @media (max-width: 500px) {
    width: 100%;
  }

  @media (max-width: 1000px) {
    width: 85%;
  }

  @media (max-width: 1300px) {
    width: 80%;
  }

  @media (max-width: 1500px) {
    width: 75%;
  }
`;

export const WideContentLayout = styled(Layout.Content)`
  width: 80%;
  margin: auto;
  margin-top: 3.5em;

  @media (max-width: 500px) {
    width: 100%;
  }

  @media (max-width: 1000px) {
    width: 85%;
  }
`;
