package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.entity.Insect;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface InsectRepository extends JpaRepository<Insect, Long> {

    Insect findByInsectId(Long insectId);

    Insect findByInsectEngName(String insectEngName);

}
