import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';

const FeatureList = [
  {
    title: 'Easy To Use',
    description: (
      <>
        Designed to be easy to understand.
      </>
    ),
  },
  {
    title: 'Eventual Consistency',
    description: (
      <>
        Ensures reliable network communication between services.
      </>
    ),
  },
  {
    title: 'Scalability & Fault Tolerance',
    description: (
      <>
        Supports multi-instance (distributed processing).
      </>
    ),
  },
  {
    title: 'Highly Configurable & Extendable',
    description: (
      <>
        Easy to expand and configure.
      </>
    ),
  },
  {
    title: 'Reduce Latency & Increase Bandwidth',
    description: (
      <>
        Using algorithms for compression, grouping and adjusting the size of the transport message, you can increase bandwidth and reduce latency.
      </>
    ),
  },
];

function Feature({Svg, title, description}) {
  return (
    <div className={clsx('col col--4')}>
      <div className="text--center">
      </div>
      <div className="text--center padding-horiz--md">
        <Heading as="h3">{title}</Heading>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures() {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}
