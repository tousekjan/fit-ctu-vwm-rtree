import React from 'react';
import {
  ContentContainer,
  WideContentContainer,
  ContentLayout,
  WideContentLayout
} from './StyledElements';

export const ContentBase = ({ title, children }) => {
  document.title = 'RTree | ' + title;

  return (
    <ContentLayout>
      <ContentContainer>{children}</ContentContainer>
    </ContentLayout>
  );
};

export const WideContent = ({ title, children }) => {
  document.title = 'RTree | ' + title;

  return (
    <WideContentLayout>
      <WideContentContainer>{children}</WideContentContainer>
    </WideContentLayout>
  );
};
