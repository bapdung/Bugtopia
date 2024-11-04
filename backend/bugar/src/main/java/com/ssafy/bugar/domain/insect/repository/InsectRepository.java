package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.entity.Insect;
import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import org.springframework.data.jpa.repository.JpaRepository;

public interface InsectRepository extends JpaRepository<Insect, Long> {

    Insect findByInsectId(Long insectId);

}
